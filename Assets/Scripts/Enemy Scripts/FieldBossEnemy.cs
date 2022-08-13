using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class FieldBossEnemy : FieldEnemy
{
    public event Action onDie;

    protected override void Die()
    {
        base.Die();

        // onDie 이벤트를 구독한 메서드가 있을 경우 메시지를 날린다.
        if (onDie != null)
            onDie();
    }

    public abstract void OnMissing();
}
