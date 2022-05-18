using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : IInflictable
{
    private Tower _fromTower;

    public float damage => _fromTower.damage;

    public BasicAttack(Tower fromTower)
    {
        _fromTower = fromTower;
    }
    
    public void Inflict(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy != null)
            enemy.TakeDamage(damage);
    }
}
