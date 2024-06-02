using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MobStat : MonoBehaviour, IDamageable
{
    int HP;
    int DEF;
    int ATK;

    private void Start() {
        HP = 25;
        ATK = 5;
    }

    private void Update() {
        if (HP<0) 
            die();
    }

    public void TakeDamage(int damage){
        UIManager.instance.hit(gameObject, damage);
        HP -= damage;
    }

    void die(){
        // 죽는 애니메이션 실행
        // 경험치 올리는 함수
    }
}
