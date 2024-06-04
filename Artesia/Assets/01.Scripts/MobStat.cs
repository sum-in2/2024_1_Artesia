using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MobStat : MonoBehaviour, IDamageable
{
    int HP;
    int DEF;
    public int ATK;
    int EXP;

    private void Start() {
        HP = 25;
        ATK = 5;
        EXP = 8;
    }

    private void Update() {
        if (HP < 0) 
            die();
    }

    public void TakeDamage(int damage){
        UIManager.instance.hit(gameObject, damage);
        BattleManager.Instance.AddLogMessage($"{gameObject.name}가 {damage} 의 데미지를 입음");
        HP -= damage;
    }

    public void die(){
        // 죽는 애니메이션 실행
        // 경험치 올리는 함수

        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        obj.GetComponent<PlayerStat>().addExp(EXP);

        EnemySpawner.instance.killEnemy(gameObject);
    }
}
