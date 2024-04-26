using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Stat{
    LV,
    HP,
    ATK,
    DEF,
    EXP,
}

public class DataManager : MonoBehaviour
{
    static DataManager Instance;
    public static DataManager instance{
        get{
            return Instance;
        }
    }
    // 캐릭터 명칭으로 접근
    public Dictionary<string, Dictionary<Stat,List<int>>> CharacterStats {get; private set;} = new Dictionary<string, Dictionary<Stat, List<int>>>();
    GameObject Player;
    
    string NametoLoadPlayerStat;

    private void OnEnable() {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
        init();
    }

    public void SaveGameData(int FileNum){
        string SaveJsonData = JsonUtility.ToJson(GameManager.instance.GameData, true);
        string filePath = Application.persistentDataPath + "/" + $"SaveData{FileNum}.json";

        File.WriteAllText(filePath, SaveJsonData);
    }

    public void LoadData(int fileNum){
        string filePath = Application.persistentDataPath + "/" + $"SaveData{fileNum}.json";
        if(File.Exists(filePath)){
            string SaveDataJson = File.ReadAllText(filePath);
            GameManager.instance.GameData = JsonUtility.FromJson<Data>(SaveDataJson);
            GameManager.instance.LoadData();
        }
    }

    void init(){ 
        Player = GameObject.FindGameObjectWithTag("Player");
        NametoLoadPlayerStat = Player.gameObject.name;
        Dictionary<Stat,List<int>> temp = roadCSVData($"{NametoLoadPlayerStat}Stat");
        
        if(temp != null)
            CharacterStats.Add(NametoLoadPlayerStat, temp);
    }

    Dictionary<Stat,List<int>> roadCSVData (string _CSVFileName){
        Dictionary<Stat,List<int>> res = new Dictionary<Stat,List<int>>();
        TextAsset csvData = Resources.Load<TextAsset>($"Player/{_CSVFileName}");
        if(csvData == null) 
            return null;

        string[] data = csvData.text.Split(new char[] { '\n' });

        for(int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++){
            res[(Stat)i] = new List<int>();
        }

        for(int i = 1; i < data.Length; i++){
            string[] element = data[i].Split(new char[] { ',' });
            for(int j = 0; j < element.Length; j++){
                res[(Stat)j].Add(int.Parse(element[j]));
            }
        }
        
        return res;
    }

    public Dictionary<Stat, List<int>> GetCharacterData(string CharacterName) {
        Dictionary<Stat, List<int>> result;
        result = CharacterStats[CharacterName];
        return result;
    }

    // saveData
    // 시작 포지션, 맵 정보, 층 정보 - 미구현, 플레이 캐릭터, 보조 캐릭터 - 미구현, 진행 중인 게임 스탯(nowHP, nowExp) ?
}