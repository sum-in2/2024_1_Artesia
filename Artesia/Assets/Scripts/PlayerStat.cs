using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableController : MonoBehaviour
{
    public static PlayableController Instance {get; private set;}
    [SerializeField] int c_HP;
    [SerializeField] int c_ATK;
    [SerializeField] int c_DEF;
    [SerializeField] int c_EXP;
    [SerializeField] int c_LV;

    int[] MAXEXP = {100,};

    void Awake(){
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
        /*c_HP = 20;
        c_ATK = 5;
        c_DEF = 2;
        c_EXP = 0;
        c_LV = 1;*/
    }

    void AddEXP(int EXP){
        c_EXP += EXP;
        if(c_EXP > MAXEXP[0]){
            c_LV++;
            PlusStat();
        }
    }

    void PlusStat(){

    }
}
