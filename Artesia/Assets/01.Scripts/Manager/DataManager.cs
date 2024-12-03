using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum Stat
{
    LV,
    HP,
    ATK,
    DEF,
    EXP,
}

public class DataManager : MonoBehaviour
{
    public Data GameData { get; set; }
    static DataManager Instance;
    public static DataManager instance
    {
        get
        {
            return Instance;
        }
    }
    // 캐릭터 명칭으로 접근
    public Dictionary<string, Dictionary<Stat, List<int>>> CharacterStats { get; private set; } = new Dictionary<string, Dictionary<Stat, List<int>>>();
    GameObject Player;

    string NametoLoadPlayerStat;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);
        init();
    }

    void SaveData()
    { // 얘랑 load는 데이터매니저로 옮겨야되지 않을까 . . .
        GameData.nowExp = Player.GetComponent<PlayerStat>().NowExp;
        GameData.nowLv = Player.GetComponent<PlayerStat>().NowLv;
        GameData.MainPlayCharacterName = Player.gameObject.name;
    }

    void LoadData()
    {
        Player.GetComponent<PlayerStat>().SetStat(
            GameData.nowExp,
            GameData.nowLv,
            GameData.MainPlayCharacterName
            );
    }

    public void SaveGameData(int FileNum)
    {
        SaveData();

        string SaveJsonData = JsonUtility.ToJson(GameData, true);
        string filePath = Application.persistentDataPath + "/" + $"SaveData{FileNum}.json";

        File.WriteAllText(filePath, SaveJsonData);
    }

    public void LoadData(int fileNum)
    {
        string filePath = Application.persistentDataPath + "/" + $"SaveData{fileNum}.json";
        if (File.Exists(filePath))
        {
            string SaveDataJson = File.ReadAllText(filePath);
            GameData = JsonUtility.FromJson<Data>(SaveDataJson);
        }
    }

    void init()
    {
        GameData = new Data();
        Player = GameObject.FindGameObjectWithTag("Player");
        NametoLoadPlayerStat = Player.gameObject.name;
        Dictionary<Stat, List<int>> temp = roadCSVData($"{NametoLoadPlayerStat}Stat");

        if (temp != null)
            CharacterStats.Add(NametoLoadPlayerStat, temp);
    }

    Dictionary<Stat, List<int>> roadCSVData(string _CSVFileName)
    {
        Dictionary<Stat, List<int>> res = new Dictionary<Stat, List<int>>();
        TextAsset csvData = Resources.Load<TextAsset>($"Player/{_CSVFileName}");
        if (csvData == null)
            return null;

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++)
        {
            res[(Stat)i] = new List<int>();
        }

        for (int i = 1; i < data.Length; i++)
        {
            string[] element = data[i].Split(new char[] { ',' });
            for (int j = 0; j < element.Length; j++)
            {
                res[(Stat)j].Add(int.Parse(element[j]));
            }
        }

        return res;
    }

    public Dictionary<Stat, List<int>> GetCharacterData(string CharacterName)
    {
        Dictionary<Stat, List<int>> result;
        result = CharacterStats[CharacterName];
        return result;
    }
}