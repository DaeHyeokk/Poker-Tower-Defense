using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePairTower : Tower
{
    [Header("Basic Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _basicSlowingAttributes;

    [Header("Special Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _specialSlowingAttributes;
    [SerializeField]
    private float _specialRange;

    public readonly string _towerName = "OnePair Tower";
    public override string towerName => _towerName;
    public override int towerIndex => 1;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        BasicAttack basicAttack = new(this);
        basicInflictorList.Add(basicAttack);
        specialInflictorList.Add(basicAttack);

        Slowing basicSlowing = new(this, _basicSlowingAttributes);
        basicInflictorList.Add(basicSlowing);

        Slowing specialSlowing = new(this, _specialSlowingAttributes);
        specialInflictorList.Add(specialSlowing);
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
            projectile.actionOnCollision += () => SpecialInflict(projectile, target, _specialRange);
        }
    }
}


/*
 * File : OnePairTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 *
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/17 TUE
 * Ÿ���� Ư�� ���� ����.
 */