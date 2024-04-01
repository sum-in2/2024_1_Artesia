using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour, IState<PlayerController>
{
    private PlayerController m_playerController;

    public void OperateEnter(PlayerController sender){
        m_playerController = sender;
    }
    public void OperateUpdate(PlayerController sender){
        return;
    }
    public void OperateExit(PlayerController sender){

    }
}