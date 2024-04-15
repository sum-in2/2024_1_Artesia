using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager Instance;
    public static GameManager instance{
        get{
            return Instance;
        }
    }

    public GameObject Player;
    public GameObject MapObject;
    public Data GameData {get; set;}

    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);

        // if(savefile 유무) LoadData
    }

    void Start(){
        
    }

    void SaveData(){ // 얘랑 load는 데이터매니저로 옮겨야되지 않을까 . . .
        GameData.nowExp = Player.GetComponent<PlayerStat>().NowExp;
        GameData.nowHp = Player.GetComponent<PlayerStat>().NowHp;
        GameData.nowLv = Player.GetComponent<PlayerStat>().NowLv;
        GameData.MainPlayCharacterName = Player.gameObject.name;
    }

    public void LoadData(){
        Player.GetComponent<PlayerStat>().NowExp = GameData.nowExp ;
        Player.GetComponent<PlayerStat>().NowHp = GameData.nowHp ;
        Player.GetComponent<PlayerStat>().NowLv = GameData.nowLv ;
        Player.gameObject.name = GameData.MainPlayCharacterName ;
    }

    public void NextStage(){
        // Stage Index 추가 예정
        EnemySpawner.instance.EnemyListClear();
        MapObject.GetComponent<MapGenerator>().InitMap();
        MapObject.GetComponent<DrawTile>().InitTile();
        Player.GetComponent<PlayerController>().MovePos();
        SaveData();
    }
}
