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
    private SpriteRenderer spriteRenderer;
    bool isFade = false;

    private void OnEnable() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HP = 25;
        ATK = 5;
        EXP = 8;
    }

    private void Update() {
        if (HP < 0 && !isFade) 
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

        StartCoroutine(DieFade());
    }

    private IEnumerator DieFade(){
        Color color = spriteRenderer.color;
        float fadeDuration = 0.5f; // 페이드 아웃 지속 시간 (초)
        isFade = true;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / fadeDuration;
            spriteRenderer.color = color;
            yield return null;
        }

        isFade = false;
        EnemySpawner.instance.killEnemy(gameObject);
    }
}