using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float speed = 0.2f;
    Vector2 OriPos;
    Vector2 targetPos;

    private bool isMoving = false;

    void Start()
    { 
    }

    void Update()
    {
        
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !isMoving)
        {
            StartCoroutine( PlayerMove());
        }
    }
    
    private IEnumerator PlayerMove()
    {
        isMoving = true;

        float elapsedTime = 0;

        OriPos = transform.position;
        targetPos = OriPos + new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        while(elapsedTime < speed){
            transform.position = Vector2.Lerp(OriPos, targetPos, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPos;
        if(!Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(speed);
        isMoving = false;
    }
}
