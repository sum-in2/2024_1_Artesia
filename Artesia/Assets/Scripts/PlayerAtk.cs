using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAtk : IState<PlayerController>
{
    Vector2 m_Dir;
    float AtkSpeed;
    Vector3 m_OriPos;
    private PlayerController m_playerController;
    float elapsedTime;
    Vector2 m_targetPos;

    public void OperateEnter(PlayerController sender){
        if(!m_playerController)
            m_playerController = sender;

        m_targetPos = sender.TargetPos;
        m_Dir = sender.Dir;
        m_OriPos = sender.transform.position;
        AtkSpeed = sender.speed / 2;
        
        elapsedTime += Time.deltaTime;
        m_playerController.transform.position = Vector2.Lerp(sender.transform.position, m_targetPos, elapsedTime / AtkSpeed);
    }
    public void OperateUpdate(PlayerController sender){
        Vector2 nowPos = m_playerController.transform.position;
        m_playerController.transform.position = Vector2.Lerp(nowPos, m_targetPos, elapsedTime / AtkSpeed);

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= AtkSpeed){ // 한번 도달하면 되돌아오게
            m_playerController.transform.position = m_targetPos;
            m_targetPos = m_OriPos;
            elapsedTime = 0;
        }
    }
    public void OperateExit(PlayerController sender){
        m_playerController.transform.position = m_OriPos;
        elapsedTime = 0;
        sender.EnemyHit = false;
        TurnManager.instance.setTurn(sender.gameObject, true);
    }
}