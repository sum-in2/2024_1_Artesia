using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{
    public void OnBackButton(){
        gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
