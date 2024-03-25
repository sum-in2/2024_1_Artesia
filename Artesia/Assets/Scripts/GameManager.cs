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

    List<Node> MapList;



    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);
    }

    public void setMapList(List<Node> nodes){
        MapList = nodes;
        /*Debug.Log("GameManager MapList");
        for(int i = 0 ; i < MapList.Count; i++) // 리스트 디버깅
            Debug.Log(MapList[i].roomRect);*/
    }

    public void NextStage(){
        // Stage Index 추가 예정
        
        MapGenerator.instance.InitMap();
        Player.GetComponent<MoveController>().MovePos();
    }
}
