using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionBtn : MonoBehaviour
{
    public GameObject optionCanvas;
    public GameObject escapeBtn;

    public void onMainBtn(){
        optionCanvas.SetActive(false);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("MainScene");
    }

    public void onBackBtn(){
        optionCanvas.SetActive(false); //OptionUI
        Time.timeScale = 1f;
    }

    public void onOptionBtn(){
        optionCanvas.SetActive(true);
        if(SceneManager.GetActiveScene().name != "BaseCamp")
            escapeBtn.SetActive(true);
        Time.timeScale = 0f;
    }

    public void onEscapeBtn(){
        optionCanvas.SetActive(false);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("BaseCamp");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MovePos(Vector3.zero);
    }
}
