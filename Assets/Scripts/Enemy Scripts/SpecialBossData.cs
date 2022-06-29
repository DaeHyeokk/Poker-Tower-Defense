using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Special Boss Data", fileName = "Special Boss Data")]
public class SpecialBossData : EnemyData
{
    public int rewardGold;          // ���� ���
    public int rewardChangeChance;    // ���� ī�庯ȯ�� ����
    public int rewardJokerCard;     // ���� ��Ŀī�� ����
}
