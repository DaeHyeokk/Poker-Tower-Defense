using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CriticalStrike : IEnemyInflictable
{
    private Tower _fromTower;
    private StringBuilder _inflictorInfo;
    private Attribute[] _attributes;

    [System.Serializable]
    public struct Attribute
    {
        public float rate;
        [Range(0,100)]
        public int chance;
    }

    private float damage => _fromTower.damage;
    private float rate => _attributes[_fromTower.level].rate;
    private float chance => _attributes[_fromTower.level].chance;

    public StringBuilder inflictorInfo => _inflictorInfo;

    public CriticalStrike(Tower fromTower, Attribute[] attributes)
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
        _inflictorInfo.Append("% 확률로 공격력의 ");
        _inflictorInfo.Append(rate.ToString());
        _inflictorInfo.Append("% 데미지");
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeDamage(_fromTower, damage * rate * 0.01f, DamageTakenType.Critical);
        else
            target.TakeDamage(_fromTower, damage, DamageTakenType.Normal);
    }
}
