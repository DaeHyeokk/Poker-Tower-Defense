using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunInflict : IInflict
{
    private float _stunTime;

    public void DoInflict(GameObject target)
    {
        target.GetComponent<Enemy>().OnStun(_stunTime);
    } 
}
