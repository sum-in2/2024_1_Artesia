using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager Instance;
    public static GameManager instance{
        get {
            return Instance;
        }
    }
    GameObject Player;
    GameObject MapObject;
    public Data GameData {get; set;}

    public int stageIndex{get; private set;} = 1;

    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    void Init(){
        // if(savefile 유무) LoadData
        GameData = new Data();
        MapObject = GameObject.FindGameObjectWithTag("Map");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start(){
        Init();
        NextStage();
    }

    public void SaveData(){ // 얘랑 load는 데이터매니저로 옮겨야되지 않을까 . . .
        GameData.nowExp = Player.GetComponent<PlayerStat>().NowExp;
        GameData.nowLv = Player.GetComponent<PlayerStat>().NowLv;
        GameData.MainPlayCharacterName = Player.gameObject.name;
    }

    public void LoadData(){
        Player.GetComponent<PlayerStat>().NowExp = GameData.nowExp ;
        Player.GetComponent<PlayerStat>().NowLv = GameData.nowLv ;
        Player.gameObject.name = GameData.MainPlayCharacterName ;
    }

    public void NextStage(){
        if(SceneManager.GetActiveScene().name != "BaseCamp"){
            StartCoroutine(UIManager.instance.FakeLoading(1f, stageIndex, "DungeonName")); // 던전네임 변수 추가 예정 ( 배열로 쓸 듯 ? )
            EnemySpawner.instance.EnemyListClear();
            MapObject.GetComponent<MapGenerator>().InitMap();
            MapObject.GetComponent<DrawTile>().InitTile();
            Player.GetComponent<PlayerController>().MovePos();
            UIManager.instance.SetDungeonInfoText("DungeonName", stageIndex);

            Camera.main.GetComponent<CameraController>().CalculateTilemapBounds();

            stageIndex++;
        }
        else
            UIManager.instance.SetDungeonInfoText("BaseCamp");
    }
}