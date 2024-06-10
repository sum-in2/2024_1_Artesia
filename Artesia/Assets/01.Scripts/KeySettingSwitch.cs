using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeySettingOnOff : MonoBehaviour
{
    [SerializeField] GameObject keySettingImage;
    private void Update() {
        
        if(SceneManager.GetActiveScene().name == "BaseCamp" && keySettingImage == true)
        {
            keySettingImage.SetActive(false);
        }
        if(SceneManager.GetActiveScene().name == "DungeonSample" && keySettingImage.activeSelf == false)
        {
            keySettingImage.SetActive(true);
        }
    }   
}
