using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public Canvas StartCanvas;
    public Canvas OptionCanvas;

    public void OnSavedGameButton(){
        SceneLoader.Instance.LoadScene("BaseCamp");
    }

    public void OnStartButton(){
        if(StartCanvas != null){
            StartCanvas.gameObject.SetActive(true);
        }
    }
}
