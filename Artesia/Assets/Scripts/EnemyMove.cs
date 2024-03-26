using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Vector2Int Dir = new Vector2Int();
    public bool isMoving{get; set;}
    private void FixedUpdate() {
        if(!isMoving) EnemyRandomMove();
    }

    void EnemyRandomMove(){
        RaycastHit2D hit;

        do{
            Dir.x = Random.Range(-1, 2);
            Dir.y = Random.Range(-1, 2);

            hit = Physics2D.Raycast(transform.position, new Vector3(Dir.x,Dir.y,0), 1, LayerMask.GetMask("Tile"));
        } while(hit); // 닿으면 true > 랜덤 다시

        StartCoroutine(RandomMove());
    }

    private IEnumerator RandomMove()
    {
        float elapsedTime = 0;
        Vector2 OriPos;
        Vector2 targetPos;
        float speed = 1f;

        OriPos = new Vector3((int)transform.position.x, (int)transform.position.y,0);
        targetPos = OriPos + Dir;

        isMoving = true;
        while(elapsedTime < speed){
            transform.position = Vector2.Lerp(OriPos, targetPos, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        this.gameObject.transform.position = targetPos;
        isMoving = false;
    }
}
