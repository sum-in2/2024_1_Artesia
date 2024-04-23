using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup LoadingCanvas;
    public bool isFade {get; private set;} = false; // 페이드중이면 이동 못하게 어차피 턴 안바뀌면 안움직이니가\
    static UIManager Instance;
    public static UIManager instance{
        get{
            return Instance;
        }
    }
    

    public TextMeshProUGUI tmp;

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

    public IEnumerator FakeLoading(float time, int stageIndex, string dungeonName){
        isFade = true;
        float timer = 0f;
        LoadingCanvas.alpha = 1f;

        if(tmp != null)
            tmp.text = dungeonName + "\n" + stageIndex + "F";

        yield return new WaitForSeconds(time);

        while(timer < time){
            yield return null;
            timer += Time.deltaTime;
            LoadingCanvas.alpha = Mathf.Lerp(1f, 0f, timer);
        }

        isFade = false;
    }
}
