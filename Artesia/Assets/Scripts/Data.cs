using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// saveData
// 시작 포지션, 맵 정보, 층 정보 - 미구현, 플레이 캐릭터, 보조 캐릭터 - 미구현, 진행 중인 게임 스탯(nowHP, nowExp), chapter
[System.Serializable]
public class Data{
    public string MainPlayCharacterName;
    public int nowExp;
    public int nowLv;
}