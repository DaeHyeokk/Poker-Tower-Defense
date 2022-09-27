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

        _rewardGold = enemyData.rewardGold;
        _rewardChangeChance = enemyData.rewardChangeChance;
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
