using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{    
    [SerializeField] Dictionary<Stat, List<int>> m_Stat;
    public int NowExp {get; set;}
    public int NowLv  {get; set;}
    public int NowHp  {get; set;}
    int Hp;
    int Def;
    int Atk;
    int Exp;

    private void Start() {
        if(DataManager.instance != null)
            m_Stat = DataManager.instance.GetCharacterData(this.gameObject.name);
    }

    private void Update() {
        if(NowExp >= Exp) LevelUp();
    }


    void InitData(){
        // if(isSaved) NowExp = savedExp 이런느낌
        NowExp = 0;
        NowLv = 1;
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
    public void SetStat(int savedHP, int savedExp, int savedLV){
        NowHp = savedHP;
        NowExp = savedExp;
        NowLv = savedLV;
    }
}
