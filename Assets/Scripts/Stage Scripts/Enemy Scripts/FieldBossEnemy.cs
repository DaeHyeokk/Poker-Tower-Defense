using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldBossEnemy : FieldEnemy
{
    public event Action onDie;

    protected override void Die(Tower fromTower)
    {
        base.Die(fromTower);

        // onDie �̺�Ʈ�� ������ �޼��尡 ���� ��� �޽����� ������.
        if (onDie != null)
            onDie();
    }

    public abstract void OnMissing();
}
