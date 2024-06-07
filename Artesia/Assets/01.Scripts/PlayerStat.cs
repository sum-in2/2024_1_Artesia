using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamageable
{    
    [SerializeField] Dictionary<Stat, List<int>> m_Stat;
    public int NowExp {get; set;} 
    public int NowLv  {get; set;}
    public int NowHp  {get; set;} = 1;
    public int Hp {get; private set;} = 1;
    public int Def{get; private set;}
    public int Atk{get; private set;}
    public int Exp{get; private set;}

    private void Start() {
        if(DataManager.instance != null)
            m_Stat = DataManager.instance.GetCharacterData(this.gameObject.name);

            InitData();
    }

    private void Update() {
        if(NowExp >= Exp) LevelUp();
        if(NowHp >= Hp) NowHp = Hp;
        if(NowHp < 0) die();
    }

    public void addHP(int addNum){
        NowHp += addNum;
    }

    public void addExp(int addExp){
        NowExp += addExp;
        BattleManager.Instance.AddLogMessage($"{gameObject.name}의 경험치가 {addExp}만큼 올랐습니다.");
    }

    void InitData(){
        // if(isSaved) { NowExp = savedExp } 이런느낌
        NowExp = 0;
        NowLv = 5;
        NowStatSetting();
    }

    void NowStatSetting(){
        Hp   = m_Stat[Stat.HP][NowLv];
        Def  = m_Stat[Stat.DEF][NowLv];
        Atk  = m_Stat[Stat.ATK][NowLv];
        Exp  = m_Stat[Stat.EXP][NowLv];
        NowHp = Hp;
    }

    void LevelUp(){
        NowLv++;
        NowExp = 0;
        NowStatSetting();
    }
    public void SetStat(int savedExp, int savedLV, string name){
        NowExp = savedExp;
        NowLv = savedLV;
        gameObject.name = name; 
    }

    public void TakeDamage(int damage){
        UIManager.instance.hit(gameObject, damage);
        addHP(-1 * damage);
    }

    void die(){
        // 캐릭터 사망 -> Game Over 출력 -> 베이스캠프 이동
    }
}
 