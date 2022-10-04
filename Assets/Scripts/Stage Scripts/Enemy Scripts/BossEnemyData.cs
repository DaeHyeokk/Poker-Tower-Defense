using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Boss Data", fileName = "Boss Data")]
public class BossEnemyData : EnemyData
{
    public string bossName;         // 보스 이름
    public int rewardGold;          // 보상 골드
    public int rewardChangeChance;    // 보상 카드변환권 개수
    public int rewardTowerLevel;    // 보상으로 지급되는 타워의 레벨
}
