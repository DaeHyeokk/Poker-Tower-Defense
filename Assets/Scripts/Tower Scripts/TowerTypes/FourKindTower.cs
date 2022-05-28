using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourKindTower : Tower
{
    [Header("Basic Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCriticalStrikeAttributes;

    [Header("Basic Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _basicSlowingAttributes;

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCriticalStrikeAttributes;

    [Header("Special Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _specialSlowingAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private float _basicRange;
    [SerializeField]
    private float _specialRange;

    private readonly string _towerName = "FourKind Tower";
    public override string towerName => _towerName;
    public override int towerIndex => 8;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCriticalStrikeAttributes);
        basicEnemyInflictorList.Add(basicCriticalStrike);

        Slowing basicSlowing = new(this, _basicSlowingAttributes);
        basicEnemyInflictorList.Add(basicSlowing);

        CriticalStrike specialCriticalStrike = new(this, _specialCriticalStrikeAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);

        Slowing specialSlowing = new(this, _specialSlowingAttributes);
        specialEnemyInflictorList.Add(specialSlowing);
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(target, _basicRange);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(target, _specialRange);
        }
    }
}


/*
 * File : FourKindTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/16 MON
 * Ÿ���� Ư�� ���� ����.
 */