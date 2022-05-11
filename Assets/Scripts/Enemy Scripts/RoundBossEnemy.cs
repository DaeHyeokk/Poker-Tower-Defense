using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnMissing()
    {
        GameManager.instance.DecreaseLife(20);

        // ������ ���� ������ ������ ��� ���� �������� ������� �ٷ� �й��Ѵ�.
        if (GameManager.instance.IsFinalRound())
            GameManager.instance.DecreaseLife(100);

        ReturnObject();
    }

    protected override void Die()
    {
        // 100���, ī�� ��ȯ�� �ֱ�
        GameManager.instance.IncreaseGold(100);
        GameManager.instance.IncreaseChangeChance(2);
        ReturnObject();
    }

    protected override void ReturnObject()
    {
        _enemySpawner.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);

    }
}
