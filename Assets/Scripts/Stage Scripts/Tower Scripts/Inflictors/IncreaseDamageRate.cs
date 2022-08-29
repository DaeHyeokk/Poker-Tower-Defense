using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class IncreaseDamageRate : ITowerInflictable
{
    private Tower _fromTower;
    private StringBuilder _inflictorInfo;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float rate;
        public float duration;
    }

    private float rate => _attributes[_fromTower.level].rate;
    private float duration => _attributes[_fromTower.level].duration;

    public StringBuilder inflictorInfo => _inflictorInfo;

    public IncreaseDamageRate(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;

        _inflictorInfo = new();
        UpdateInflictorInfo();
    }

    public void UpdateInflictorInfo()
    {
        _inflictorInfo.Clear();
        _inflictorInfo.Append(duration.ToString());
        _inflictorInfo.Append("초 동안 공격력 ");
        _inflictorInfo.Append(rate.ToString());
        _inflictorInfo.Append("% 증가");
    }

    public void Inflict(Tower target)
    {
        if (target == null) return;

        target.TakeIncreaseDamageRate(rate, duration);
    }
}
