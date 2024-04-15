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

    int TurnCnt;

    void Awake(){
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);
            
        TurnCnt = 0;
    }
    
    private void Start() {
        MobList = EnemySpawner.instance.enemies;
    }

    private void Update(){
        if(CheckUnitTurn()){
            TurnCnt++;
            setTurn(Player, false);
            foreach(GameObject Obj in MobList){
                setTurn(Obj, false);
            }
        }

        if(TurnCnt > 8){
            EnemySpawner.instance.RandomSpawnEnemy();
            TurnCnt = 0;
        }
    }

    public void setTurn(GameObject obj, bool input){
        ITurn TurnTemp = obj.GetComponent<ITurn>();
        if(TurnTemp != null)
            TurnTemp.PlayedTurn = input;
        else
            Debug.Log("ITurn 상속받지 않은 오브젝트 : " + obj.name);
    }

    bool CheckUnitTurn(){
        // 모든 유닛의 턴이 실행 됐으면 true > 턴 switch 함수
        // 유닛의 턴 상태는 완료 > ture, 대기 > false
        if(!Player.GetComponent<ITurn>().PlayedTurn)
            return false;
        foreach(GameObject Obj in MobList){
            if(!Obj.activeSelf)
                continue;
            if(!Obj.GetComponent<ITurn>().PlayedTurn)
                return false;
        }
        return true;
    }
}
