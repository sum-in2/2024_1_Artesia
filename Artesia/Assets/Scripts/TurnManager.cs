using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //bool PlayerTurn 
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
