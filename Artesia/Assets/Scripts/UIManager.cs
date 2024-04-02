using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager Instance;
    public static UIManager instance{
        get{
            return Instance;
        }
    }

    [SerializeField] GameObject NextStageUI;

    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveNextStageUI(bool bActive){
        NextStageUI.SetActive(bActive);
    }
}
