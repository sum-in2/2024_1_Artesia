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
    public GameObject Player;
    public GameObject MapObject;
    public Data GameData {get; set;}

    public int stageIndex{get; private set;} = 1;

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
        NextStage();
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

        if(SceneManager.GetActiveScene().name != "BaseCamp"){
            StartCoroutine(UIManager.instance.FakeLoading(1f, stageIndex++, "DungeonName")); // 던전네임 변수 추가 예정 ( 배열로 쓸 듯 ? )
            EnemySpawner.instance.EnemyListClear();
            MapObject.GetComponent<MapGenerator>().InitMap();
            MapObject.GetComponent<DrawTile>().InitTile();
            Player.GetComponent<PlayerController>().MovePos();
        }
    }

    //Fade 관련 함수 아마 분리 시킬듯 
    // UI카메라로 옮겨서 화면 다 덮고 하는게
}
