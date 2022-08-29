using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Special Boss Data", fileName = "Special Boss Data")]
public class SpecialBossData : EnemyData
{
    public int rewardGold;          // 보상 골드
    public int rewardChangeChance;    // 보상 카드변환권 개수
    public int rewardJokerCard;     // 보상 조커카드 개수
}
