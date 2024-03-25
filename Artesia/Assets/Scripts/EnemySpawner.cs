using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static EnemySpawner m_instance;

    public static EnemySpawner instance{
        get{
            return m_instance;
        }
    }

    void Awake() {
        if(m_instance == null)
            m_instance = this; 
        else if(m_instance != this)
            Destroy(this.gameObject);
    }

    [SerializeField] GameObject enemyPrefab;

    public void SpawnEnemy(){
        List<Node> rooms = GameManager.instance.MapList;
        foreach(Node room in rooms){
            if(room != MapGenerator.instance.startRoom){
                Vector2 temp1 = room.roomRect.center;
                Vector2Int temp2 = MapGenerator.instance.MapSize;
                Vector3 roomCenter = new Vector3(((int)temp1.x - temp2.x / 2), ((int)temp1.y - temp2.y / 2), 0);
                Instantiate(enemyPrefab, roomCenter, Quaternion.identity);
            }
        }
    }
}
