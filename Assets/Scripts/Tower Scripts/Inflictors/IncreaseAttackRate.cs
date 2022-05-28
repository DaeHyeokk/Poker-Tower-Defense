using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackRate : ITowerInflictable
{
    private Tower _fromTower;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float rate;
        public float duration;
    }

    private float rate => _attributes[_fromTower.level].rate;
    private float duration => _attributes[_fromTower.level].duration;

    public IncreaseAttackRate(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;
    }

    public void Inflict(Tower target)
    {
        if (target == null) return;

        target.TakeIncreaseAttackRate(rate, duration);
    }
}
