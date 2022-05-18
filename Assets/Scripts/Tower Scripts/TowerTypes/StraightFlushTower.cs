using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightFlushTower : Tower
{
    [Header("Basic Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCritAttributes;

    [Header("Basic Stun")]
    [SerializeField]
    private Stun.Attribute[] _basicStunAttributes;

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Special Stun")]
    [SerializeField]
    private Stun.Attribute[] _specialStunAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private float _basicRange;
    [SerializeField]
    private float _specialRange;

    public override string towerName => "StraightFlush Tower";

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        basicInflictorList.Add(basicCriticalStrike);

        Stun basicStun = new(this, _basicStunAttributes);
        basicInflictorList.Add(basicStun);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialInflictorList.Add(specialCriticalStrike);

        Stun specialStun = new(this, _specialStunAttributes);
        specialInflictorList.Add(specialStun);
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(projectile, target, _basicRange);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(projectile, target, _specialRange);
        }
    }
}


/*
 * File : StraightFlushTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 */