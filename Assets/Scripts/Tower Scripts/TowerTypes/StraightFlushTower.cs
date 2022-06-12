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

    private readonly string _towerName = "��Ƽ�� Ÿ��";

    public override string towerName => _towerName;
    public override int towerIndex => 9;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        baseEnemyInflictorList.Add(basicCriticalStrike);

        Stun basicStun = new(this, _basicStunAttributes);
        baseEnemyInflictorList.Add(basicStun);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);

        Stun specialStun = new(this, _specialStunAttributes);
        specialEnemyInflictorList.Add(specialStun);
    }

    protected override void UpdateDetailInfo()
    {
        base.UpdateDetailInfo();

        detailBaseAttackInfo.Insert(0, "[���� ����]\n");
        detailSpecialAttackInfo.Insert(0, "[���� ����]\n");
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BaseInflict(target, _basicRange);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(target, _specialRange);
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
 * 
 * Update : 2022/05/17 TUE
 * Ÿ���� Ư�� ���� ����.
 */