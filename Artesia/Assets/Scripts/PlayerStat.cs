using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{    
    [SerializeField] Dictionary<Stat, List<int>> m_Stat;
    int NowExp;
    int NowLv;
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
    }

    void LevelUp(){
        NowLv++;
        NowExp = 0;
        NowStatSetting();
    }
}
