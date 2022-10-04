using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Boss Data", fileName = "Boss Data")]
public class BossEnemyData : EnemyData
{
    public string bossName;         // ���� �̸�
    public int rewardGold;          // ���� ���
    public int rewardChangeChance;    // ���� ī�庯ȯ�� ����
    public int rewardTowerLevel;    // �������� ���޵Ǵ� Ÿ���� ����
}
