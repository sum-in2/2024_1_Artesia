using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private bool isAttacking = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (gameObject.GetComponent<PlayerController>().EnemyHit)
            {
                if (!isAttacking && gameObject.GetComponent<Collider2D>().bounds.center == other.bounds.center)
                {   // kill enemy는 수정 예정.
                    isAttacking = true;
                    UIManager.instance.hit(other.gameObject, 1);
                    EnemySpawner.instance.killEnemy(other.gameObject);
                    StartCoroutine(ResetAttackAfterDelay());
                }
            }
            else
            {
                if (!isAttacking && gameObject.GetComponent<Collider2D>().bounds.center == other.bounds.center)
                {
                    isAttacking = true;
                    UIManager.instance.hit(gameObject, 1);
                    gameObject.GetComponent<PlayerStat>().addHP(-1);
                    StartCoroutine(ResetAttackAfterDelay());
                }
            }
        }
    }

    IEnumerator ResetAttackAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 공격 후 0.5초 대기
        isAttacking = false;
    }
}