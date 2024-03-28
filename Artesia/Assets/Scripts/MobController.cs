using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 0.2f;
    Vector2 OriPos;
    Vector2 targetPos;
    Vector3 dir;
    public bool mobTurn;
    void Update()
    {
        dir.x = Random.Range(-1,2);
        dir.y = Random.Range(-1,2);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1, LayerMask.GetMask("Tile"));

        if (dir != Vector3.zero && !hit && !mobTurn)
        {
            mobTurn = true;
            StartCoroutine(MobMove());
        }
    }
    
    private IEnumerator MobMove()
    {
        float elapsedTime = 0;

        OriPos = new Vector3((int)transform.position.x, (int)transform.position.y,0);
        targetPos = OriPos + new Vector2(dir.x, dir.y);

        while(elapsedTime < speed){
            transform.position = Vector2.Lerp(OriPos, targetPos, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        this.gameObject.transform.position = targetPos;
    }
}
