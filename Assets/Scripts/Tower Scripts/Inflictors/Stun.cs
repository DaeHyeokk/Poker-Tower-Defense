using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : IInflictable
{
    private Tower _fromTower;
    private Enemy _target;
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

    public void Inflict(GameObject target)
    {
        if (target.TryGetComponent(out _target))
            if (chance > Random.Range(0, 100))
                _target.TakeStun(duration);
    }
}
