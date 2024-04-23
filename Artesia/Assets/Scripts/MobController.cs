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
        toPlayerPath = gameObject.GetComponent<AStarPathfinder>().StartPathfinding(transform.position, PlayerPos); 
    }

    public void setListPath(){
        toPlayerPath = null;
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
                if(toPlayerPath.Count == 1)
                {
                    PlayedTurn = true;
                    return;
                }
                TargetPos = GetComponent<AStarPathfinder>().ConvertMapToWorldPosition(toPlayerPath[0]);
            }
            SM.SetState(dicState[MobState.Move]);
        }
    }

    public void hit(int Dmg){
        GameObject DmgText = Resources.Load<GameObject>("Prefabs/DmgText");
        if(DmgText == null){
            Debug.Log("dmgtext 로드 실패");
            return;
        }
        GameObject obj = Instantiate(DmgText);
        obj.GetComponent<DmgText>().Init(Dmg, transform.position);
    }
}
