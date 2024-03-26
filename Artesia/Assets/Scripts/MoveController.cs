using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MoveController : MonoBehaviour
{
    public float speed = 0.2f;
    Vector2 OriPos;
    Vector2 targetPos;
    Vector2 Dir;

    private bool isMoving = false;

    void Start() {
        MoveStartPos();
    }

    public void MoveStartPos(){
        transform.localPosition = MapGenerator.instance.StartPos;
    }

    void FixedUpdate()
    {
        Dir.x = Input.GetAxisRaw("Horizontal");
        Dir.y = Input.GetAxisRaw("Vertical");

        Debug.DrawRay(transform.position, new Vector3(Dir.x,Dir.y,0), Color.black, 0.3f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(Dir.x,Dir.y,0), 1, LayerMask.GetMask("Tile"));

        if ((Dir.x != 0 || Dir.y != 0) && !isMoving && !hit)
        {
            StartCoroutine( PlayerMove());
        }
    }
    
    private IEnumerator PlayerMove()
    {
        isMoving = true;

        float elapsedTime = 0;

        OriPos = new Vector3((int)transform.position.x, (int)transform.position.y,0);
        targetPos = OriPos + Dir;

        while(elapsedTime < speed){
            transform.position = Vector2.Lerp(OriPos, targetPos, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        this.gameObject.transform.position = targetPos;
        if(!Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(speed);
        isMoving = false;
    }
}
