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

        // 생성할 Enemy의 체력 설정 (현재 스테이지 난이도에 비례)
        maxHealth = enemyData.health * StageManager.instance.bossEnemyHpPercentage;
        health = maxHealth;

        _rewardGold = enemyData.rewardGold;
        _rewardChangeChance = enemyData.rewardChangeChance;

        _rewardStringBuilder.Set(_rewardGold, _rewardChangeChance);
    }

    protected override void Die(Tower fromTower)
    {
        base.Die(fromTower);

        // onDie 이벤트를 구독한 메서드가 있을 경우 메시지를 날린다.
        if (onDie != null)
            onDie();
    }

    public abstract void OnMissing();
}
