using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobMove : MonoBehaviour, IState<MobController>
{
    private MobController m_mobController;
    public float speed = 0.2f;
    public Vector2 targetPos { get; set;}
    Vector3 dir;
    RaycastHit2D hit;
    float elapsedTime;

    public void OperateEnter(MobController sender){
        if(!m_mobController)
            m_mobController = sender;

        elapsedTime = 0;
        do{
            dir.x = Random.Range(-1,2);
            dir.y = Random.Range(-1,2);
            hit = Physics2D.Raycast(m_mobController.transform.position, dir, 1, LayerMask.GetMask("Tile"));
        } while(hit);

        Vector3 startPos = new Vector3((int)m_mobController.transform.position.x, (int)m_mobController.transform.position.y,0);
        targetPos = startPos + new Vector3(dir.x, dir.y, 0);
    }
    public void OperateUpdate(MobController sender){
        Vector3 nowPos = m_mobController.transform.position;
        m_mobController.transform.position = Vector2.Lerp(nowPos, targetPos, elapsedTime / speed);
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= speed){
            m_mobController.transform.position = targetPos;
            OperateExit(sender);
            // state 변경 ??
            // 아니면 statemachine에서
        }
    }
    public void OperateExit(MobController sender){

    }

}

    /*
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
    }*/