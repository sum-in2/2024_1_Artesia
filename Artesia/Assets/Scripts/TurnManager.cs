using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    List<GameObject> MobList;
    
    static TurnManager Instance;
    public static TurnManager instance{
        get{
            return Instance;
        }
    }

    void Awake(){
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);
    }
    
    private void Start() {
        MobList = EnemySpawner.instance.enemies;
    }

    private void Update(){
        if(CheckUnitTurn()){
            setPlayerTurn(false);
            foreach(GameObject Obj in MobList){
                Obj.GetComponent<MobController>().PlayedTurn = false;
            }
        }
    }

    public void setPlayerTurn(bool input){
        Player.GetComponent<PlayerController>().PlayedTurn = input;
    }

    bool CheckUnitTurn(){
        // 모든 유닛의 턴이 실행 됐으면 true > 턴 switch 함수
        // 유닛의 턴 상태는 완료 > ture, 대기 > false
        if(!Player.GetComponent<PlayerController>().PlayedTurn)
            return false;
        foreach(GameObject Obj in MobList){
            if(!Obj.activeSelf)
                continue;
            if(!Obj.GetComponent<MobController>().PlayedTurn)
                return false;
        }
        return true;
    }
}
