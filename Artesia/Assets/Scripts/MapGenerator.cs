using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] float minDevideRate;
    [SerializeField] float maxDevideRate;
    [SerializeField] private int maxDepth;
    [SerializeField] Tilemap tileMap;
    [SerializeField] Tilemap tileMap_Room;
    [SerializeField] Tile RoomTile;
    [SerializeField] Tile WallTile;
    [SerializeField] Tile outTile;
    Node StartRoom;
    Node StairRoom;
    void Awake() {
        FillBackGround();
        Node root = new Node(new RectInt(0,0,mapSize.x,mapSize.y));
        Divide(root,0);

        GenerateRoom(root, 0);
        GenerateLoad(root, 0);
        FillWall();
        
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
    
    void FillWall(){
        for(int i = 0; i < mapSize.x; i++)
            for(int j = 0; j < mapSize.y; j++)
                if(tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                    for(int x = -1 ; x <= 1; x++)
                        for(int y = -1; y <= 1; y++){
                            if(x == 0 && y == 0) continue;

                            if(tileMap.GetTile(new Vector3Int(i - mapSize.x /2 + x, j - mapSize.y / 2 + y, 0)) == RoomTile){
                                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), WallTile);
                                break;
                            }
                        }
    }

    private void FillRoom(RectInt rect, int n) { 
    for(int i = rect.x; i< rect.x + rect.width; i++)
            for(int j = rect.y; j < rect.y + rect.height; j++){
                
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), RoomTile);
            }
    }
}

