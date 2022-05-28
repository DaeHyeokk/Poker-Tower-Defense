using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowing : IEnemyInflictable
{
    private Tower _fromTower;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float rate;
        public float duration;
        [Range(0,100)]
        public int chance;
    }

    private float rate => _attributes[_fromTower.level].rate;
    private float duration => _attributes[_fromTower.level].duration;
    private int chance => _attributes[_fromTower.level].chance;

    public Slowing(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeSlowing(rate, duration);
    }
}
