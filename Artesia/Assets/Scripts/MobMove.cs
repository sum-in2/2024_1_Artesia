using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobMove :  IState<MobController>
{
    private MobController m_mobController;
    float m_speed;
    Vector2 m_targetPos;
    float elapsedTime;

    public void OperateEnter(MobController sender){
        if(!m_mobController)
            m_mobController = sender;
        elapsedTime = 0;

        m_speed = m_mobController.speed;
        m_targetPos = m_mobController.TargetPos;
    }
    public void OperateUpdate(MobController sender){
        Vector3 nowPos = m_mobController.transform.position;
        m_mobController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / m_speed);
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= m_speed){
            m_mobController.transform.position = m_targetPos;
        }
    }
    public void OperateExit(MobController sender){
        m_mobController.transform.position = m_targetPos;
        TurnManager.instance.setTurn(sender.gameObject, true);
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