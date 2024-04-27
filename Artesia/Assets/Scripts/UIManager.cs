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
    

    public TextMeshProUGUI LoadingStageText;
    public TextMeshProUGUI DungeonInfo;
    
    Dictionary<string, GameObject> DicUi;
    [SerializeField] GameObject NextStageUI;
    [SerializeField] GameObject statusUI;

    void Awake()
    {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);

        DicUi = new Dictionary<string, GameObject>();
        DicUi.Add("NextStage", NextStageUI);
        DicUi.Add("Status", statusUI);
    }

    public void SetActiveUI(string UIName, bool bActive){
        if(DicUi[UIName] == null)
        {
            Debug.Log("UI가 사전에 업음");
            return;
        }
        DicUi[UIName].SetActive(bActive);
    }

    public void SetActiveUI(GameObject UIobj, bool bActive){
        UIobj.SetActive(bActive);
    }

    public IEnumerator FakeLoading(float time, int stageIndex, string dungeonName){
        isFade = true;
        float timer = 0f;
        LoadingCanvas.alpha = 1f;

        if(LoadingStageText != null)
            LoadingStageText.text = dungeonName + "\n" + stageIndex + "F";

        yield return new WaitForSeconds(time);

        while(timer < time){
            yield return null;
            timer += Time.deltaTime;
            LoadingCanvas.alpha = Mathf.Lerp(1f, 0f, timer / time);
        }

        isFade = false;
    }
    
    public void SetDungeonInfoText(string DungeonName, int stageIndex){
        DungeonInfo.text = DungeonName + " " + stageIndex + "F";
    }

    public void SetDungeonInfoText(string DungeonName){
        DungeonInfo.text = DungeonName;
    }

    public void hit(GameObject obj, int Dmg){
        GameObject DmgText = Resources.Load<GameObject>("Prefabs/DmgText");
        if(DmgText == null){
            Debug.Log("dmgtext 로드 실패");
            return;
        }
        GameObject textObj = Instantiate(DmgText);
        textObj.GetComponent<DmgText>().Init(Dmg, obj.transform.position);
    }
}
