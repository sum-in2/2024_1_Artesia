using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private void Update() {
        if(CheckObjectTurn())
            Invoke("NextTurn",1f);
    }

    bool CheckObjectTurn(){ // turn false
        if(!Player.GetComponent<MoveController>().playerTurn_move)
            return false;
        
        return true;
    }

    void NextTurn(){
        Player.GetComponent<MoveController>().playerTurn_move = false;
    }
}
