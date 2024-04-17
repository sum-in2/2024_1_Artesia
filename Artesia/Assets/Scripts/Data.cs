using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// saveData
// 시작 포지션, 맵 정보, 층 정보 - 미구현, 플레이 캐릭터, 보조 캐릭터 - 미구현, 진행 중인 게임 스탯(nowHP, nowExp), chapter
// 만약에 끌 때 저장이 된다면 몹 정보도 저장해야 함
public class Data{
    //public string nowSceneName // Scene 로딩때 써야함
    public string MainPlayCharacterName{get; set;}
    public int nowHp{get; set;}
    public int nowExp{get; set;}
    public int nowLv{get; set;}
}