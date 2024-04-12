using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{    
    // public int HP { get; private set; }
    // public int EXP { get; private set; }    
    // public int DEF { get; private set;}
    // public int ATK { get; private set;}
    // public int LV { get; private set;}
    
    [SerializeField] Dictionary<Stat, List<int>> m_Stat;

    private void Start() {
        if(DataManager.instance != null)
            myStatSelect(gameObject.name);
    
    }

    void myStatSelect(string name){
        m_Stat = DataManager.instance.GetCharacterData(name);
    }
}