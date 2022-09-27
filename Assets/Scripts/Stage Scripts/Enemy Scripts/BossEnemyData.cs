using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Boss Data", fileName = "Boss Data")]
public class BossEnemyData : EnemyData
{
    public int rewardGold;          // 보상 골드
    public int rewardChangeChance;    // 보상 카드변환권 개수
}
