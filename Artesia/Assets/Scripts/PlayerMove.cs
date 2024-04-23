using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMove : IState<PlayerController>
{
    private PlayerController m_playerController;
    float speed;
    float elapsedTime;
    Vector2 m_targetPos;

    public void OperateEnter(PlayerController sender){
        if(!m_playerController)
            m_playerController = sender;
        elapsedTime = 0;

        speed = m_playerController.speed;
        m_targetPos = m_playerController.TargetPos;

        if(Mathf.Abs(sender.Dir.x) == 1){                                                 // 기본이 왼쪽방향
            sender.gameObject.GetComponent<SpriteRenderer>().flipX = (sender.Dir.x == 1); // 1이면 오른쪽 방향이니 플립켜야함
        }
    }
    
    public void OperateUpdate(PlayerController sender){
        Vector2 nowPos = m_playerController.transform.position;
        m_playerController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / speed);

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= speed){
            m_playerController.transform.position = m_targetPos;
        }
    }
    
    public void OperateExit(PlayerController sender){
        m_playerController.transform.position = m_targetPos;
        TurnManager.instance.setTurn(sender.gameObject, true);
    }
}