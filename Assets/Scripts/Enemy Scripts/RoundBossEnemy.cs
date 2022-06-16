using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBossEnemy : FieldBossEnemy
{
    public override void OnMissing()
    {
        // ���� ������ ������ ��� ���ӿ��� �й��Ѵ�.
        GameManager.instance.DefeatGame();

        ReturnObject();
    }

    protected override void Die()
    {
        base.Die();
        // 100���, ī�� ��ȯ�� �ֱ�
        GameManager.instance.gold += 100;
        GameManager.instance.changeChance += 2;

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        EnemySpawner.instance.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
