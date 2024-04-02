using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static EnemySpawner m_instance;
    [SerializeField] GameObject enemyPrefab;
    List<GameObject> Enemies;
    List<GameObject> enemyPool;

    public List<GameObject> enemies
    {
        get { return Enemies; }
    }

    public static EnemySpawner instance
    {
        get
        {
            return m_instance;
        }
    }

    public int poolSize = 10; // 오브젝트 풀 크기

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(this.gameObject);

        Enemies = new List<GameObject>();
        enemyPool = new List<GameObject>();

        // 초기에 오브젝트 풀에 enemyPrefab을 생성하여 추가합니다.
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false); // 비활성화 상태로 시작
            enemyPool.Add(enemy);
        }
    }

    public void EnemyListClear()
    {
        foreach (GameObject Enemy in Enemies)
        {
            Enemy.GetComponent<EnemyMove>().isMoving = false;
            Enemy.SetActive(false); // 활성화된 상태를 false로 변경하여 비활성화합니다.
        }

        Enemies.Clear();
    }

    public void SpawnEnemy()
    {
        List<Node> rooms = GameManager.instance.MapList;
        
        foreach (Node room in rooms)
        {

            if (room != MapGenerator.instance.startRoom)
            {
                Vector2 temp1 = room.roomRect.center;
                Vector2Int temp2 = MapGenerator.instance.MapSize;
                Vector3 roomCenter = new Vector3(((int)temp1.x - temp2.x / 2), ((int)temp1.y - temp2.y / 2), 0);

                // 오브젝트 풀에서 비활성화된 적 캐릭터를 찾아서 활성화하여 재사용합니다.
                GameObject enemy = GetPooledEnemy();
                if (enemy != null)
                {
                    enemy.transform.position = roomCenter;
                    enemy.SetActive(true);
                    enemies.Add(enemy);
                }
            }
        }
    }

    GameObject GetPooledEnemy()
    {
        // 오브젝트 풀에서 비활성화된 적 캐릭터를 찾아 반환합니다.
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (!enemyPool[i].activeInHierarchy)
            {
                return enemyPool[i];
            }
        }

        // 비활성화된 적 캐릭터가 없을 경우 null 반환합니다.
        return null;
    }
}