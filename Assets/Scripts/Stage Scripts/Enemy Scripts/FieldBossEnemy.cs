using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldBossEnemy : FieldEnemy
{
    public event Action onDie;

    public virtual void Setup(BossEnemyData enemyData)
    {
        base.Setup(enemyData);

        // ������ Enemy�� ü�� ���� (���� �������� ���̵��� ���)
        maxHealth = enemyData.health * StageManager.instance.bossEnemyHpPercentage;
        health = maxHealth;

        _rewardGold = enemyData.rewardGold;
        _rewardChangeChance = enemyData.rewardChangeChance;

        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance);
    }

    protected override void Die(Tower fromTower)
    {
        base.Die(fromTower);

        // onDie �̺�Ʈ�� ������ �޼��尡 ���� ��� �޽����� ������.
        if (onDie != null)
            onDie();
    }

    public abstract void OnMissing();
}
