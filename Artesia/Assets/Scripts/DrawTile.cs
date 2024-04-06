using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawTile : MonoBehaviour
{
    enum RoomInfo{ // 타일 배열에 관한 열거
        Out,
        Room,
        Wall,
        Stair,
    }

    [SerializeField] Tilemap tileMap; // 배경
    [SerializeField] Tilemap stairMap; // 상호작용 타일 이름 바꿀듯?
    Dictionary<RoomInfo, Tile> dicTile = new Dictionary<RoomInfo, Tile>();
    [SerializeField] Tile RoomTile;
    [SerializeField] Tile WallTile;
    [SerializeField] Tile outTile;
    [SerializeField] Tile stairTile;
    int [,] MapTileInfo;
    Vector2Int m_mapSize;
    Vector3Int m_stairPos;
    // Start is called before the first frame update
    void Start()
    {
        initDic();
        InitTile();
    }

    public void InitTile(){
        initMember();
        FillMap();
    }

    void initMember(){
        MapTileInfo = MapGenerator.instance.MapInfoArray;
        m_mapSize = MapGenerator.instance.MapSize;

        if(m_stairPos != null)
            if(stairMap.GetTile(m_stairPos) == stairTile)
                stairMap.SetTile(m_stairPos, null);
        m_stairPos = MapGenerator.instance.stairPos;
    }

    void initDic(){
        dicTile.Add(RoomInfo.Out, outTile);
        dicTile.Add(RoomInfo.Room, RoomTile);
        dicTile.Add(RoomInfo.Wall, WallTile);
        dicTile.Add(RoomInfo.Stair, stairTile);
    }

    void FillMap(){
        //FillBackGround();
        DrawMap();
        DrawStair();
    }

    void DrawStair(){
        mySetTile(stairMap, m_stairPos, RoomInfo.Stair);
    }

    void DrawMap(){
        for (int i = 0; i < m_mapSize.y; i++){
            for(int j = 0; j < m_mapSize.x; j++){
                mySetTile(tileMap, j, i, (RoomInfo)MapTileInfo[i,j]);
            }
        }
    }

    private void FillBackGround()
    {
        for(int i = -10; i<m_mapSize.x + 10; i++)
            for(int j = -10; j < m_mapSize.y +10; j++)
                tileMap.SetTile(new Vector3Int(i - m_mapSize.x / 2, j - m_mapSize.y / 2, 0), outTile);
    }

    void mySetTile(Tilemap tilemap, int x, int y, RoomInfo roomInfo){
        tilemap.SetTile(new Vector3Int(x  - m_mapSize.x / 2, y  - m_mapSize.y / 2, 0), dicTile[roomInfo]);
    }
    void mySetTile(Tilemap tilemap, Vector3Int pos, RoomInfo roomInfo){
        tilemap.SetTile(pos, dicTile[roomInfo]);
    }
}