using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowInflict : IInflict
{
    private float _slowingRate;
    public void DoInflict(GameObject target)
    {
        target.GetComponent<Enemy>().OnSlow(_slowingRate);
    }
}
