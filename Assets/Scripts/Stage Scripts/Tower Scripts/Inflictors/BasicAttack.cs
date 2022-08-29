using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BasicAttack : IEnemyInflictable
{
    private Tower _fromTower;
    private StringBuilder _inflictorInfo = new("공격력의 100% 데미지");

    private float damage => _fromTower.damage;
    public StringBuilder inflictorInfo => _inflictorInfo;

    public BasicAttack(Tower fromTower)
    {
        _fromTower = fromTower;
    }

    public void UpdateInflictorInfo()
    {
        return;
    }

    public void Inflict(Enemy target)
    {
        if (target == null) return;

        target.TakeDamage(_fromTower, damage, DamageTakenType.Normal);
    }
}
