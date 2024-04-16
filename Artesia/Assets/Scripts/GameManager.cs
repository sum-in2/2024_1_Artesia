using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager Instance;
    public static GameManager instance{
        get{
            return Instance;
        }
    }

    [SerializeField] SpriteRenderer dummyNextScene;
    public GameObject Player;
    public GameObject MapObject;
    public bool isFade = false; // 페이드중이면 이동 못하게 어차피 턴 안바뀌면 안움직이니가
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
        SetFadeImage(dummyNextScene, true);
        EnemySpawner.instance.EnemyListClear();
        MapObject.GetComponent<MapGenerator>().InitMap();
        MapObject.GetComponent<DrawTile>().InitTile();
        Player.GetComponent<PlayerController>().MovePos();
        StartCoroutine(Fadein(dummyNextScene, 2f));
    }
    
    void SetFadeImage(SpriteRenderer image, bool Alpha){
        isFade = true;
        Color color = image.color;
        if(Alpha) color.a = 1f;
        else color.a = 0f;
        image.color = color;
    }

    IEnumerator Fadein(SpriteRenderer image,float time){
        Color color = image.color;
        while (color.a > 0f){
            color.a -= Time.deltaTime / time;
            image.color = color;
            yield return null;
        }
        isFade = false;
    }
}
