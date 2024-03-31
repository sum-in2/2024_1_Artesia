using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobController : MonoBehaviour
{
    public enum MobState{
        Idle,
        Move,
        Atk,
    }

    private Dictionary<MobState, IState<MobController>> dicState = new Dictionary<MobState, IState<MobController>>();
    private StateMachine<MobController> SM;

    void Awake(){
        IState<MobController> idle = new MobIdle();
        IState<MobController> move = new MobMove();
        IState<MobController> atk = new MobAtk();

        dicState.Add(MobState.Idle, idle);
        dicState.Add(MobState.Move, move);
        dicState.Add(MobState.Atk, atk);

        //SM = new StateMachine<MobController>(this, dicState[MobState.Idle]);
        SM = new StateMachine<MobController>(this, dicState[MobState.Move]); // 테스트 - move로 스타트
    }

    private void Update() { // turn 매니저에서 턴을 관리 할거니까 setstate도 흠 .. ... ..
        //SM.SetState(dicState[MobState.Idle]);

        if(SM.CurState == dicState[MobState.Move]){
            /*if(this.GetComponent<MobMove>().targetPos == (Vector2)transform.position){
                SM.SetState(dicState[MobState.Idle]);
            }*/
            SM.SetState(dicState[MobState.Idle]);
            Debug.Log("ㅇㅇ?");
        }

        SM.DoOperateUpdate();
    }
}
