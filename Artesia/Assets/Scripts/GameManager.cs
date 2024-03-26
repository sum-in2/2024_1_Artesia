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

    public List<Node> MapList{get;set;}

    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void NextStage(){
        // Stage Index 추가 예정
        
        MapGenerator.instance.InitMap();
        Player.GetComponent<MoveController>().MoveStartPos();
    }
}
