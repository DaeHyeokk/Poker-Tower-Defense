using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseReceivedDamageRate : IInflictable
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

    public IncreaseReceivedDamageRate(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;
    }


    public void Inflict(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy != null)
            if (chance > Random.Range(0, 100))
                enemy.TakeIncreaseReceivedDamage(rate, duration);
    }
}
