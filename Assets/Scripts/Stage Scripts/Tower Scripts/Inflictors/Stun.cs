using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Stun : IEnemyInflictable
{
    private Tower _fromTower;
    private StringBuilder _inflictorInfo;
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

    public StringBuilder inflictorInfo => _inflictorInfo;

    public Stun(Tower fromTower, Attribute[] attributes)
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
        _inflictorInfo.Append("% Ȯ���� ");
        _inflictorInfo.Append(duration.ToString());
        _inflictorInfo.Append("�� ���� �� ����");
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        if (chance > Random.Range(0, 100))
            target.TakeStun(duration);
    }
}
