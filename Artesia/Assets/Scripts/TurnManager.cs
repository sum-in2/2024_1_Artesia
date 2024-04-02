using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    
    static TurnManager Instance;
    public static TurnManager instance{
        get{
            return Instance;
        }
    }

    void Awake(){
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);
    }
}
