using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
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

    public Vector2 OriPos;

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
        toPlayerPath = gameObject.GetComponent<AStarPathfinder>().StartPathfinding(transform.position, PlayerPos); 
    }

    public void setListPath(){
        toPlayerPath = null;
    }

    private void Update() {
        if(!PlayedTurn){
            Move();
        }

        if((Vector2)transform.position == OriPos && SM.CurState == dicState[MobState.Atk])
            SM.SetState(dicState[MobState.Idle]);

        if(TargetPos == (Vector2)transform.position && SM.CurState == dicState[MobState.Move]){
            SM.SetState(dicState[MobState.Idle]);
        }
        SM.DoOperateUpdate();
    }

    void Move(){
        if(SM.CurState == dicState[MobState.Idle]){
            
            Collider2D hit;

            if(toPlayerPath == null)
            {
                do{
                    Dir = new Vector2(Random.Range(-1,2), Random.Range(-1,2));
                    //hit = Physics2D.Raycast(transform.position, Dir, 1, LayerMask.GetMask("Tile"));
                    hit = Physics2D.OverlapPoint(new Vector2(transform.position.x + Dir.x, transform.position.y + Dir.y));
                } while(hit);

                TargetPos = Dir + (Vector2) transform.position;
            }
            else
            {
                TargetPos = GetComponent<AStarPathfinder>().ConvertMapToWorldPosition(toPlayerPath[0]);
                if(toPlayerPath.Count == 1)
                {
                    OriPos = transform.position;
                    SM.SetState(dicState[MobState.Atk]);
                    return;
                }
                toPlayerPath.RemoveAt(0);
            }
            SM.SetState(dicState[MobState.Move]);
        }
    }
}
