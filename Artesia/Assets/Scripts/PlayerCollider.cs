using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Enemy" && gameObject.GetComponent<PlayerController>().EnemyHit){
            // other.GetComponent<MobStat>().~~~ << >> playerstat
            // 알고리즘 짜면 아마 플레이어 캐릭터랑 겹칠 일은 없을거 같은데 현재로서는 겹친 상태에서 공격 시 닿아서 없어지는 현상있음
            EnemySpawner.instance.killEnemy(other.gameObject);
        }
    }
}
