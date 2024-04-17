using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour, ITurn
{
    public enum PlayerState{
        Idle,
        Move, 
        Atk,
    }

    private Dictionary<PlayerState, IState<PlayerController>> dicState = new Dictionary<PlayerState, IState<PlayerController>>();
    private StateMachine<PlayerController> SM;
    public Vector2 OriPos { get; private set;}
    public Vector2 Dir { get; private set;} = Vector2.down;
    public Vector2 TargetPos {get; private set;}
    [SerializeField][Range(0.0001f, 1f)][Tooltip("커질수록 느려짐")] float Speed = 0.2f;
    public float speed {
        get { return Speed;}
    }
    public bool isMoving { get; private set;} = false;
    public bool PlayedTurn { get; set; }
    public bool EnemyHit { get; set; } = false;


    void Awake(){
        IState<PlayerController> idle = new PlayerIdle();
        IState<PlayerController> move = new PlayerMove();
        IState<PlayerController> atk = new PlayerAtk();

        dicState.Add(PlayerState.Idle, idle);
        dicState.Add(PlayerState.Move, move);
        dicState.Add(PlayerState.Atk, atk);

        SM = new StateMachine<PlayerController>(this, dicState[PlayerState.Idle]);
    }

    void Start() {
        MovePos();

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if((Vector2)transform.position == TargetPos && SM.CurState == dicState[PlayerState.Move])
            SM.SetState(dicState[PlayerState.Idle]);
        
        if((Vector2)transform.position == OriPos && SM.CurState == dicState[PlayerState.Atk])
            SM.SetState(dicState[PlayerState.Idle]);

        SM.DoOperateUpdate();
    }

    void OnMove(InputValue value){
        if(!PlayedTurn && !UIManager.instance.isFade){
            Vector2 input = value.Get<Vector2>();
            if(input != Vector2.zero && SM.CurState == dicState[PlayerState.Idle]){ // 
                input = DirControl(input); 
                Vector3Int OriPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
                RaycastHit2D hit = Physics2D.Raycast((Vector2Int)OriPos, input, 1, LayerMask.GetMask("Tile"));

                if(!hit){
                    Dir = input;
                    TargetPos = OriPos + new Vector3(Dir.x, Dir.y, 0);
                    SM.SetState(dicState[PlayerState.Move]);
                }
            }
            
        }
    }

    void OnAtk(InputValue value){
        if(!PlayedTurn && SM.CurState == dicState[PlayerState.Idle] && !UIManager.instance.isFade){
            OriPos = transform.position;
            if(Dir != Vector2.zero)
                Dir = DirControl(Dir);
            
            if(Physics2D.Raycast(OriPos, Dir, 1, LayerMask.GetMask("Enemy")))
                EnemyHit = true;
                        
            TargetPos = OriPos + Dir;
            SM.SetState(dicState[PlayerState.Atk]);
        }
    }

    Vector2 DirControl(Vector2 dir){ //
        Vector2 Result = dir;
        float x, y;
        x = Mathf.Abs(Result.x);
        y = Mathf.Abs(Result.y);

        if((x+y) > 1){
            Result.x = Result.x > 0 ? Mathf.CeilToInt(Result.x) : Mathf.FloorToInt(Result.x);
            Result.y = Result.y > 0 ? Mathf.CeilToInt(Result.y) : Mathf.FloorToInt(Result.y);
        }

        return Result;
    }
    
    public void MovePos(){
        SM.SetState(dicState[PlayerState.Idle]);
        if(MapGenerator.instance != null)
            transform.position = MapGenerator.instance.StartPos;
    }
}