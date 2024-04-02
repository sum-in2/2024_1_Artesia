using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;    

public class MobController : MonoBehaviour
{
    public enum MobState{
        Idle,
        Move,
        Atk,
    }

    public Vector2 Dir{get; private set;}
    public Vector2 TargetPos {get; private set;}
    [SerializeField][Range(0.0001f, 2f)][Tooltip("커질수록 느려짐")] float Speed = 1.5f;
    public float speed {
        get { return Speed;}
    }
    public bool PlayedTurn {get; set;}

    private Dictionary<MobState, IState<MobController>> dicState = new Dictionary<MobState, IState<MobController>>();
    private StateMachine<MobController> SM;

    void Awake(){
        IState<MobController> idle = new MobIdle();
        IState<MobController> move = new MobMove();
        IState<MobController> atk = new MobAtk();

        dicState.Add(MobState.Idle, idle);
        dicState.Add(MobState.Move, move);
        dicState.Add(MobState.Atk, atk);

        SM = new StateMachine<MobController>(this, dicState[MobState.Idle]); // 테스트 - move로 스타트
    }

    private void Update() { // turn 매니저에서 턴을 관리 할거니까 setstate도 흠 .. ... ..
        // 
        //SM.SetState(dicState[MobState.Idle]);
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
            Debug.Log("22");
            PlayedTurn = true;
            RaycastHit2D hit;
            do{
                Dir = new Vector2(Random.Range(-1,2), Random.Range(-1,2));
                hit = Physics2D.Raycast(transform.position, Dir, 1, LayerMask.GetMask("Tile"));
            } while(hit);

            TargetPos = Dir + (Vector2) transform.position;
            SM.SetState(dicState[MobState.Move]);
        }
    }
}
