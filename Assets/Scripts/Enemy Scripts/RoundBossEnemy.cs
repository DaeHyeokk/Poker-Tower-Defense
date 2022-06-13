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
        GameManager.instance.life -= 20;

        // ������ ���� ������ ������ ��� ���� �������� ������� �ٷ� �й��Ѵ�.
        if (GameManager.instance.IsFinalWave())
            GameManager.instance.life -= 100;

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
        enemySpawner.roundEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
