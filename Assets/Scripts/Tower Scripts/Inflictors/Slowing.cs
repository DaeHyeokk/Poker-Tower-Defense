using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Slowing : IEnemyInflictable
{
    private Tower _fromTower;
    private StringBuilder _inflictorInfo;
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

    public StringBuilder inflictorInfo => _inflictorInfo;

    public Slowing(Tower fromTower, Attribute[] attributes)
    {
        _fromTower = fromTower;
        _attributes = attributes;

        _inflictorInfo = new();
        UpdateInflictorInfo();
    }

    public void UpdateInflictorInfo()
    {
        _inflictorInfo.Clear();
        _inflictorInfo.Append(chance.ToString());
        _inflictorInfo.Append("% 확률로 ");
        _inflictorInfo.Append(duration.ToString());
        _inflictorInfo.Append("초 동안 적의 이동속도 ");
        _inflictorInfo.Append(rate.ToString());
        _inflictorInfo.Append("% 감소");
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeSlowing(rate, duration);
    }
}
