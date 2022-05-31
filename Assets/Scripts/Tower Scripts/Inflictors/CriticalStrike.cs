using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalStrike : IEnemyInflictable
{
    private Tower _fromTower;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float rate;
        [Range(0,100)]
        public int chance;
    }

    public float damage => _fromTower.damage;
    public float rate => _attributes[_fromTower.level].rate;
    public float chance => _attributes[_fromTower.level].chance;

    public CriticalStrike(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeDamage(damage * rate * 0.01f, DamageTakenType.Critical);
        else
            target.TakeDamage(damage, DamageTakenType.Normal);
    }
}
