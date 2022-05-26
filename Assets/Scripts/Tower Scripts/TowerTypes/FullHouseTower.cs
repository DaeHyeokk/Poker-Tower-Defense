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
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ 
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/16 MON
 * Ÿ���� Ư�� ���� ����.
 */
