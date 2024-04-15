using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    static EnemySpawner m_instance;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int poolSize = 20;
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

    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(this.gameObject);

        Enemies = new List<GameObject>();
        enemyPool = new List<GameObject>();
        AddEnemyToPool(enemyPrefab, poolSize);
    }

    void AddEnemyToPool(GameObject Prefab, int EnemyCnt){
        for (int i = 0; i < EnemyCnt; i++){
            GameObject enemy = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false); // 비활성화 상태로 시작
            enemy.name = ("Enmey" + i);
            enemy.transform.SetParent(this.transform);
            enemyPool.Add(enemy);
        }
    }

    public void killEnemy(GameObject enemy){
        GameObject killedObject = null;
        foreach (var temp in enemies){ // 임시구현
            if(temp.name == enemy.name){ // 실제 계산은 플레이어쪽에서 계산 되고 얘는 이렇게 둘거 같은데 모르겠네
                enemy.GetComponent<MobController>().setStateToIdle();
                enemy.SetActive(false);
                killedObject = enemy;
            }
        }
        if(killedObject != null)
            enemies.Remove(killedObject);
    }

    public void EnemyListClear()
    {
        foreach (GameObject Enemy in Enemies)
        {
            Enemy.GetComponent<MobController>().setStateToIdle();
            Enemy.SetActive(false); // 활성화된 상태를 false로 변경하여 비활성화합니다.
        }
    }

    public void ActiveFromPool(){
        List<Node> rooms = MapGenerator.instance.rooms;
        
        foreach (Node room in rooms){

            if (room != MapGenerator.instance.startRoom){
                Vector2 temp1 = room.roomRect.center;
                Vector2Int temp2 = MapGenerator.instance.MapSize;
                Vector3 roomCenter = new Vector3(((int)temp1.x - temp2.x / 2), ((int)temp1.y - temp2.y / 2), 0);

                SpawnEnemy(roomCenter);
            }
        }
    }

    GameObject GetPooledEnemy(){
        // 오브젝트 풀에서 비활성화된 적 캐릭터를 찾아 반환합니다.
        for (int i = 0; i < enemyPool.Count; i++){
            if (!enemyPool[i].activeInHierarchy){
                return enemyPool[i];
            }
        }

        // 비활성화된 적 캐릭터가 없을 경우 null 반환합니다.
        return null;
    }

    public void RandomSpawnEnemy(){
        List<Node> rooms = MapGenerator.instance.rooms;
        int NumberOfEnemyToSpawn = Random.Range(0, rooms.Count);
        int Cnt = 0;
        
        // Camera Rect ~
        foreach (Node room in rooms){
            if(room.IntersectsOtherObject(Camera.main.GetComponent<CameraController>().screenRect)){
                continue;
            }

            Vector2 temp1 = room.roomRect.center;
            Vector2Int temp2 = MapGenerator.instance.MapSize;
            Vector3 roomCenter = new Vector3(((int)temp1.x - temp2.x / 2), ((int)temp1.y - temp2.y / 2), 0);

            SpawnEnemy(roomCenter);
            if(++Cnt > NumberOfEnemyToSpawn) break;
        }
    }

    void SpawnEnemy(Vector3 SpawnPosition){
        GameObject enemy = GetPooledEnemy();
        if (enemy != null){
            enemy.transform.position = SpawnPosition;
            enemy.SetActive(true);
            enemies.Add(enemy);
        }
    }
}