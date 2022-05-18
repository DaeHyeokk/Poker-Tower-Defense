using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlushTower : Tower
{
    [Header("Basic Stun")]
    [SerializeField]
    private Stun.Attribute[] _basicStunAttributes;

    [Header("Special Stun")]
    [SerializeField]
    private Stun.Attribute[] _specialStunAttributes;

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private int _specialRange;

    private int specialRange => _specialRange;
    public override string towerName => "Flush Tower";

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        BasicAttack basicAttack = new(this);
        basicInflictorList.Add(basicAttack);

        Stun basicStun = new(this, _basicStunAttributes);
        basicInflictorList.Add(basicStun);

        Stun specialStun = new(this, _specialStunAttributes);
        specialInflictorList.Add(specialStun);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialInflictorList.Add(specialCriticalStrike);
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(projectile, target);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(projectile, target, specialRange);
        }
    }
}


/*
 * File : FlushTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 Tower를 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 */