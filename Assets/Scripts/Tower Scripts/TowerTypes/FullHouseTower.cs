using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHouseTower : Tower
{
    [Header("Basic CriticalStrike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCritAttributes;

    [Header("Special IncreaseDamageRate")]
    [SerializeField]
    private IncreaseDamageRate.Attribute[] _specialIncreaseDamageRateAttributes;

    [Header("Special IncreaseAttackRate")]
    [SerializeField]
    private IncreaseAttackRate.Attribute[] _specialIncreaseAttackRateAttributes;

    private readonly string _towerName = "FullHouse Tower";
    public override string towerName => _towerName;
    public override int towerIndex => 7;
    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        basicInflictorList.Add(basicCriticalStrike);

        IncreaseDamageRate specialIncreaseDamageRate = new(this, _specialIncreaseDamageRateAttributes);
        specialInflictorList.Add(specialIncreaseDamageRate);

        IncreaseAttackRate specialIncreaseAttackRate = new(this, _specialIncreaseAttackRateAttributes);
        specialInflictorList.Add(specialIncreaseAttackRate);
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
            projectile.actionOnCollision += () => BasicInflict(projectile, target);

            SpecialInflict(this);
        }
    }
}


/*
 * File : FullHouseTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트 
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 * 
 * Update : 2022/05/16 MON
 * 타워의 특수 공격 구현.
 */
