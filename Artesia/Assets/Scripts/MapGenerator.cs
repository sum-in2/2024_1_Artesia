using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
//using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    static MapGenerator m_instance; // 싱글톤
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minDevideRate;
    [SerializeField] float maxDevideRate;
    [SerializeField] int maxDepth;
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tilemap Test;
    [SerializeField] Tile RoomTile;
    [SerializeField] Tile WallTile;
    [SerializeField] Tile outTile;
    [SerializeField] Tile stairTile;
    Node StartRoom;
    Vector3Int startPos;
    Vector3Int stairPos;
    public Vector3Int StartPos{
        get{ return startPos; }
    }
    public static MapGenerator instance {
        get {
            return m_instance;
        }
    }

    Node StairRoom;

    int StartDepth;
    int StairDepth;

    void Awake() {   
        m_instance = this; 
        InitMap();
    }

    public void InitMap(){
        initMember();
        FillBackGround();
        Node root = new Node(new RectInt(0,0,mapSize.x,mapSize.y));
        Divide(root,0);

        InitRoom(root, 0);
        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall();
        Debug.Log(startPos);
    }

    void initMember(){
        StairDepth = 0;
        StartDepth = 0;
        if(Test.GetTile(stairPos) == stairTile)
            Test.SetTile(stairPos, null);
    }

    void InitRoom(Node Tree, int n){

        if(Random.Range(0,2) == 0){
            StartRoom = Tree.leftNode;
            StairRoom = Tree.rightNode;
        } else {
            StartRoom = Tree.rightNode;
            StairRoom = Tree.leftNode;
        }
        
        InitStartRoom(StartRoom, n+1, Random.Range(0, (int)Mathf.Pow(2,maxDepth-1)));
        InitStairRoom(StairRoom, n+1, Random.Range(0, (int)Mathf.Pow(2,maxDepth-1)));
    }

    void InitStartRoom(Node Tree, int n, int maxStartDepth){
        if(n == maxDepth) {
            if(StartDepth == maxStartDepth){
                StartRoom = Tree;
            }
            StartDepth++;
            return;
        }
        InitStartRoom(Tree.leftNode, n + 1, maxStartDepth);
        InitStartRoom(Tree.rightNode, n + 1, maxStartDepth);
    }

   void InitStairRoom(Node Tree, int n, int maxStairDepth){
        if(n == maxDepth) {
            if(StairDepth == maxStairDepth){
                StairRoom = Tree;
            }
            StairDepth++;
            return;
        }
        InitStairRoom(Tree.leftNode, n + 1, maxStairDepth);
        InitStairRoom(Tree.rightNode, n + 1, maxStairDepth);
    }

    private void FillBackGround(){
        for(int i = -10; i<mapSize.x + 10; i++)
            for(int j = -10; j < mapSize.y +10; j++)
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
    }

    void Divide(Node Tree,int n){
        if (n == maxDepth) return;

        int maxLength = Mathf.Max(Tree.nodeRect.width, Tree.nodeRect.height);
        int split = Mathf.RoundToInt(Random.Range(maxLength*minDevideRate,maxLength*maxDevideRate));
        if(Tree.nodeRect.width >= Tree.nodeRect.height){
            Tree.leftNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y, split, Tree.nodeRect.height));
            Tree.rightNode = new Node(new RectInt(Tree.nodeRect.x+split, Tree.nodeRect.y, Tree.nodeRect.width-split, Tree.nodeRect.height));
        }
        else{
            Tree.leftNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y, Tree.nodeRect.width,split));
            Tree.rightNode = new Node(new RectInt(Tree.nodeRect.x, Tree.nodeRect.y + split, Tree.nodeRect.width , Tree.nodeRect.height-split));
        }
        Tree.leftNode.parNode = Tree;
        Tree.rightNode.parNode = Tree;

        Divide(Tree.leftNode, n+1);
        Divide(Tree.rightNode, n+1);
    }

    private RectInt GenerateRoom(Node Tree, int n){
        RectInt rect;
        if(n == maxDepth){
            rect = Tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            int height = Random.Range(rect.height / 2, rect.height - 1);

            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);

            rect = new RectInt(x, y, width, height);
            FillRoom(rect, n);
            if(Tree == StartRoom || Tree == StairRoom) FillRoom(Tree, rect);
        }
        else{
            Tree.leftNode.roomRect = GenerateRoom(Tree.leftNode, n+1);
            Tree.rightNode.roomRect = GenerateRoom(Tree.rightNode, n+1);
            rect = Tree.leftNode.roomRect;
        }
        return rect;
    }

    private void GenerateLoad(Node Tree, int n){
        if(n == maxDepth) return;

        Vector2Int leftNodeCenter = Tree.leftNode.center;
        Vector2Int rightNodeCenter = Tree.rightNode.center;

        for(int i = Mathf.Min(leftNodeCenter.x , rightNodeCenter.x); i < Mathf.Max(leftNodeCenter.x,rightNodeCenter.x); i++)
            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, leftNodeCenter.y - mapSize.y /2, 0), RoomTile);

        for(int i = Mathf.Min(leftNodeCenter.y , rightNodeCenter.y); i < Mathf.Max(leftNodeCenter.y,rightNodeCenter.y); i++)
            tileMap.SetTile(new Vector3Int(rightNodeCenter.x - mapSize.x / 2, i - mapSize.y / 2, 0), RoomTile);

        GenerateLoad(Tree.leftNode, n + 1);
        GenerateLoad(Tree.rightNode, n + 1);
    }
    
    void FillWall() {
        for (int i = 0; i < mapSize.x; i++) {
            for (int j = 0; j < mapSize.y; j++) { // i, j 맵 전체 순회
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile) { // 순회한 위치가 바깥 타일이면
                    if (ShouldPlaceWall(i, j)) { // 조건 확인 메서드 사용
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), WallTile); // 벽 생성
                    }
                }
            }
        }
    }

    bool ShouldPlaceWall(int i, int j) {
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) { // 현재 자리를 중심으로 3*3 순회
                if (x == 0 && y == 0) continue; // 현재 자리 검사는 안해도 됨

                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == RoomTile) { // 순회한 위치가 룸타일이면
                    return true; // 벽을 생성해야 함
                }
            }
        }
        return false; // 벽을 생성하지 않음
    }

    private void FillRoom(RectInt rect, int n) { 
        for(int i = rect.x; i< rect.x + rect.width; i++)
                for(int j = rect.y; j < rect.y + rect.height; j++){
                    tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), RoomTile);
                }
    }

    void FillRoom(Node Tree, RectInt rect){
        Vector3Int Pos = new Vector3Int((int)rect.center.x - mapSize.x / 2, (int)rect.center.y - mapSize.y / 2, 0);
        if(Tree == StartRoom){
            startPos = Pos;
        }
        else if(Tree == StairRoom) {
            stairPos = Pos;
            Test.SetTile(Pos, stairTile);
        }
            /* 디버깅 용
            for(int i = rect.x; i < rect.x + rect.width; i++){  
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, rect.y - mapSize.y/2, 0), startTile);
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, rect.y + rect.height - mapSize.y/2, 0), startTile);
            }

            for(int i = rect.y; i < rect.y + rect.height; i++){
                tileMap.SetTile(new Vector3Int(rect.x - mapSize.x / 2, i - mapSize.y/2, 0), startTile);
                tileMap.SetTile(new Vector3Int(rect.x + rect.width - mapSize.x / 2, i - mapSize.y/2, 0), startTile);
            }
            */
            
            // tileMap.SetTile(new Vector3Int((int)rect.center.x - mapSize.x / 2, (int)rect.center.y - mapSize.y / 2, 0), startTile); //settile 할 필요 없이 플레이어블 오브젝트 포지션만 옮기면 될듯
    }

    
}