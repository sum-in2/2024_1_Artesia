using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerSkill : IState<PlayerController>
{
    private PlayerController m_playerController;
    public int damage = 15;
    public float skillRange = 1f;
    public float skillDelay = 0.3f;
    string skillName = "칼날 회오리";

    private float delayTimer;

    public void OperateEnter(PlayerController sender)
    {
        if (!m_playerController)
            m_playerController = sender;

        // 스킬 정보 초기화 ex) 대미지, 범위, 딜레이, 이펙트(이름으로 호출할거니까?)

        BattleManager.Instance.AddLogMessage($"레이나 {skillName} 사용!"); // 스킬이름 추가해야할듯

        ActivateSkill();
        delayTimer = 0f;
    }
    public void OperateUpdate(PlayerController sender)
    {
        delayTimer += Time.deltaTime;
        if (delayTimer >= skillDelay)
        { // 이펙트 딜레이 대기해야함 일단은 제외
            sender.isSkillActive = false;
        }
    }
    public void OperateExit(PlayerController sender)
    {
        TurnManager.instance.EndPlayerTurn();
    }

    private void ActivateSkill()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_playerController.transform.position, skillRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }

}
