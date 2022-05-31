using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : IEnemyInflictable
{
    private Tower _fromTower;

    public float damage => _fromTower.damage;

    public BasicAttack(Tower fromTower)
    {
        _fromTower = fromTower;
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        target.TakeDamage(damage, DamageTakenType.Normal);
    }
}
