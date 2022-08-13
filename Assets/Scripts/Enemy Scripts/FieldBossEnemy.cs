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

        // onDie �̺�Ʈ�� ������ �޼��尡 ���� ��� �޽����� ������.
        if (onDie != null)
            onDie();
    }

    public abstract void OnMissing();
}
