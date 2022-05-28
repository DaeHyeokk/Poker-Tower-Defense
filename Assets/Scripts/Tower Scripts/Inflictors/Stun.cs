using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : IEnemyInflictable
{
    private Tower _fromTower;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float duration;
        [Range(0,100)]
        public int chance;
    }

    private float duration => _attributes[_fromTower.level].duration;
    private int chance => _attributes[_fromTower.level].chance;

    public Stun(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeStun(duration);
    }
}
