using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInflict : IInflict
{
    private float _damage;

    public void DoInflict(GameObject target)
    {
        target.GetComponent<Enemy>().OnDamage(_damage);
    }
}
