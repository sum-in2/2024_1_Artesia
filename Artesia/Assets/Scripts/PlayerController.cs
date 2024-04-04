using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Vector2 TargetPos {get; private set;}
    public Vector2 Dir { get; private set;}
    [SerializeField][Range(0.0001f, 2f)][Tooltip("커질수록 느려짐")] float Speed = 0.2f;
    public float speed {
        get { return Speed;}
    }
    public bool isMoving { get; private set;} = false;
    public bool PlayedTurn { get; set; }

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
    }

    private void Update() {
        if(PlayedTurn && SM.CurState != dicState[PlayerState.Idle])
            SM.SetState(dicState[PlayerState.Idle]);

        SM.DoOperateUpdate();
    }

    void OnMove(InputValue value){
        if(!PlayedTurn && SM.CurState == dicState[PlayerState.Idle]){
            Vector3 OriPos = new Vector3((int)transform.position.x, (int)transform.position.y, 0);
            Vector2 input = value.Get<Vector2>();
            RaycastHit2D hit = Physics2D.Raycast(OriPos, input, 1, LayerMask.GetMask("Tile"));

            if(input != Vector2.zero && !hit){
                Dir = input;
                TargetPos = OriPos + new Vector3(Dir.x, Dir.y, 0);
                SM.SetState(dicState[PlayerState.Move]);
            }
        }
    }
    
    public void MovePos(){
        SM.SetState(dicState[PlayerState.Idle]);
        transform.localPosition = MapGenerator.instance.StartPos;
    }
}