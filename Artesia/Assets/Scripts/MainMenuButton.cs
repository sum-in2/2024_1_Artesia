using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public void OnStartButton(){
        //tmpro는 접근이 다른가범
        //string buttontext = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        SceneLoader.Instance.LoadScene("DungeonSample");
    }
}
