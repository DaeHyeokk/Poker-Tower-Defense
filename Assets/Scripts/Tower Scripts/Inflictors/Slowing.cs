using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowing : IInflictable
{
    private Tower _fromTower;
    private Enemy _target;
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

    public void Inflict(GameObject target)
    {
        if (target.TryGetComponent(out _target))
            if(chance > Random.Range(0,100))
                _target.TakeSlowing(rate, duration);
    }
}
