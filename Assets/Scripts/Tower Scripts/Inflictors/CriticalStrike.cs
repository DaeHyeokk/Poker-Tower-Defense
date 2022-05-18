using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalStrike : IInflictable
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

    public void Inflict(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (chance > Random.Range(0, 100))
                enemy.TakeDamage(damage * rate * 0.01f);
            else
                enemy.TakeDamage(damage);
        }
    }

}
