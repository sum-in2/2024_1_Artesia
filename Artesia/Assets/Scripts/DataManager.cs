using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake() {
        if(Instance == null)
            Instance = this; 
        else if(Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);

        CharacterStats.Add("Reina", roadCSVData("ReinaStat"));
    }

    Dictionary<Stat,List<int>> roadCSVData (string _CSVFileName){
        Dictionary<Stat,List<int>> res = new Dictionary<Stat,List<int>>();
        TextAsset csvData = Resources.Load<TextAsset>($"Player/{_CSVFileName}");
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
    // 현재 위치, 맵 정보, 플레이 캐릭터, 보조 캐릭터, 레벨 ?
}

