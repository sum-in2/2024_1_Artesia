using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IAblity
{
    public int HP { get; private set; }
    public int EXP { get; private set; }    
    public int DEF { get; private set;}
    public int ATK { get; private set;}
    public int LV { get; private set;}

    int[] MAXEXP = {100,};

    void Awake(){
        HP = 20;
        ATK = 5;
        DEF = 2;
        EXP = 0;
        LV = 1;
    }

    void AddEXP(int EXP){
        EXP += EXP;
        if(EXP > MAXEXP[LV - 1]){
            LV++;
            PlusStat();
        }
    }

    void PlusStat(){

    }
}
