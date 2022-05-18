using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTower : Tower
{
    [Header("Basic Increase Received Damage Rate")]
    [SerializeField]
    private IncreaseReceivedDamageRate.Attribute[] _basicIRDRateAttributes;

    [Header("Special Increase Damage Rate")]
    [SerializeField]
    private IncreaseDamageRate.Attribute[] _specialIDRateAttributes;
    [SerializeField]
    private float _specialIDRateRange;

    public override string towerName => "Mountain Tower";

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Multiple;

        BasicAttack basicAttack = new(this);
        basicInflictorList.Add(basicAttack);

        IncreaseReceivedDamageRate basicIRDRate = new(this, _basicIRDRateAttributes);
        basicInflictorList.Add(basicIRDRate);

        IncreaseDamageRate specialIDRate = new(this, _specialIDRateAttributes);
        specialInflictorList.Add(specialIDRate);
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
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(projectile, target);

            SpecialInflict(this, _specialIDRateRange);
        }
    }
}


/*
 * File : MountainTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 */