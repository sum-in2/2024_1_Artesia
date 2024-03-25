using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageButton : MonoBehaviour
{
    

    public void onClickYesButton(){ 
        UIManager.instance.SetActiveNextStageUI(false);
        GameManager.instance.NextStage();
    }
    public void onClickNoButton(){ 
        UIManager.instance.SetActiveNextStageUI(false);
    }
}
