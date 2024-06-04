using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.VersionControl;

public class BattleManager : MonoBehaviour
{   
    public static BattleManager Instance { get; private set;}
    public TMP_Text battleLogText;
    private bool isBattleActive = false;
    private const int maxLines = 4;
    private Queue<string> logLines = new Queue<string>();
    public ScrollRect scrollRect;
    
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        battleLogText.text = "";

        DontDestroyOnLoad(gameObject);
    }

    public void startBattle()
    {
        isBattleActive = true;
    }

    public void EndBattle(){
        isBattleActive = false;
    }

    public void AddLogMessage(string message)
    {
        logLines.Enqueue(message);

        if(logLines.Count > maxLines)
        {
            logLines.Dequeue();
        }

        UpdateBattleLog();
    }

    private void UpdateBattleLog()
    {
        battleLogText.text = string.Join("\n", logLines);

        if(logLines.Count >= maxLines)
        {
            StartCoroutine(ScrollAndRemoveFirstLine());
        }
    }

    IEnumerator ScrollAndRemoveFirstLine()
    {
        Vector3 originalPos = scrollRect.content.localPosition;
        Vector3 targetPos = originalPos + new Vector3(0, 75f);

        float elapsedTime = 0f;
        float duration = 0.2f;

        while(elapsedTime < duration)
        {
            scrollRect.content.localPosition = Vector3.Lerp(originalPos, targetPos, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollRect.content.localPosition = targetPos;


        RemoveFirstLine();

        scrollRect.content.localPosition = originalPos;
    }
    
    private void RemoveFirstLine()
    {
        string[] lines = battleLogText.text.Split('\n');
        string updatedLog = string.Join("\n", lines, 1, lines.Length - 1);
        battleLogText.text = updatedLog;
    }
}
