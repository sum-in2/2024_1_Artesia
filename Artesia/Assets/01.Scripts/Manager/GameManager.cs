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
    public static GameManager instance
    {
        get
        {
            return Instance;
        }
    }
    GameObject Player;
    GameObject MapObject;

    public string dungeonName;
    public int stageIndex { get; private set; } = 1;

    public ItemSpawner itemSpawner;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    void Init()
    {
        // if(savefile 유무) LoadData
        MapObject = GameObject.FindGameObjectWithTag("Map");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Init();
        NextStage();
    }

    public void NextStage()
    {
        if (SceneManager.GetActiveScene().name != "BaseCamp")
        {
            StartCoroutine(UIManager.instance.FakeLoading(1f, stageIndex, dungeonName));
            EnemySpawner.instance.EnemyListClear();
            MapObject.GetComponent<MapGenerator>().InitMap();
            MapObject.GetComponent<DrawTile>().InitTile();
            Player.GetComponent<PlayerController>().MovePos();
            itemSpawner.SpawnItemsInRandomRooms();
            UIManager.instance.SetDungeonInfoText(dungeonName, stageIndex);

            Camera.main.GetComponent<CameraController>().CalculateTilemapBounds();

            stageIndex++;
        }
        else
        {
            UIManager.instance.SetDungeonInfoText("BaseCamp");
        }
    }
}
