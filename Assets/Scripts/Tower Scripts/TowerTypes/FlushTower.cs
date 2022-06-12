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
    private float _specialRange;

    private readonly string _towerName = "플러쉬 타워";

    public override string towerName => _towerName;
    public override int towerIndex => 6;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        BasicAttack basicAttack = new(this);
        baseEnemyInflictorList.Add(basicAttack);

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

        detailSpecialAttackInfo.Insert(0, "[범위 공격]\n");
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BaseInflict(target);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(target, _specialRange);
        }
    }
}


/*
 * File : FlushTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 Tower를 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 * Update : 2022/05/16 MON
 * 타워의 특수 공격 구현.
 * 
 */