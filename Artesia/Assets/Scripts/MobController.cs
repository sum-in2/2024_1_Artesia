using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;    

public class MobController : MonoBehaviour, ITurn
{
    enum MobState{
        Idle,
        Move,
        Atk,
    }

    public Vector2 Dir{get; private set;}
    public Vector2 TargetPos {get; private set;}
    [SerializeField][Range(0.0001f, 1f)][Tooltip("커질수록 느려짐")] float Speed = 1f;
    public float speed {
        get { return Speed;}
    }
    public bool PlayedTurn {get; set;}

    private Dictionary<MobState, IState<MobController>> dicState = new Dictionary<MobState, IState<MobController>>();
    private StateMachine<MobController> SM;

    List<Vector2Int> toPlayerPath;

    void Awake(){
        IState<MobController> idle = new MobIdle();
        IState<MobController> move = new MobMove();
        IState<MobController> atk = new MobAtk();

        dicState.Add(MobState.Idle, idle);
        dicState.Add(MobState.Move, move);
        dicState.Add(MobState.Atk, atk);

        SM = new StateMachine<MobController>(this, dicState[MobState.Idle]);
    }

    public void setStateToIdle(){
        SM.SetState(dicState[MobState.Idle]);
    }

    public void setListPath(Vector3 PlayerPos){
        if((toPlayerPath = gameObject.GetComponent<AStarPathfinder>().StartPathfinding(transform.position, PlayerPos)) != null)
            foreach(var path in toPlayerPath)
                Debug.Log(gameObject.name + " " + path);
    }

    private void Update() {
        // if(캐릭터 감지 함수){Move();}
        if(!PlayedTurn)
            Move();
        if(TargetPos == (Vector2)transform.position){
            SM.SetState(dicState[MobState.Idle]);
        }

        SM.DoOperateUpdate();
    }

    void Move(){
        if(SM.CurState == dicState[MobState.Idle]){
            if(IsPlayerNearby()) {
                PlayedTurn = true;
                return;
            }
            
            RaycastHit2D hit;

            if(toPlayerPath == null)
            {
                do{
                    Dir = new Vector2(Random.Range(-1,2), Random.Range(-1,2));
                    hit = Physics2D.Raycast(transform.position, Dir, 1, LayerMask.GetMask("Tile"));
                } while(hit);

                TargetPos = Dir + (Vector2) transform.position;
            }
            else
            {
                TargetPos = GetComponent<AStarPathfinder>().ConvertMapToWorldPosition(toPlayerPath[1]);
                Debug.Log(TargetPos);
            }
            SM.SetState(dicState[MobState.Move]);
        }
    }
    bool IsPlayerNearby(){
        Vector2[] dirs = {Vector2.down, Vector2.up, Vector2.right, Vector2.left, new Vector2(1,1), new Vector2(-1,1), new Vector2(1, -1), new Vector2(-1, -1)};
        foreach (Vector2 dir in dirs){
            if(Physics2D.Raycast(transform.position, dir, 1, LayerMask.GetMask("Player"))){
                return true;
            }
        }

        return false;
    }
}
