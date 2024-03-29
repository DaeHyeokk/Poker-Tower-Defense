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

    [Header("Inflict Range")]
    [SerializeField]
    private float _specialRange;

    public override int towerIndex => 1;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        BasicAttack basicAttack = new(this);
        baseEnemyInflictorList.Add(basicAttack);
        specialEnemyInflictorList.Add(basicAttack);

        Slowing basicSlowing = new(this, _basicSlowingAttributes);
        baseEnemyInflictorList.Add(basicSlowing);

        Slowing specialSlowing = new(this, _specialSlowingAttributes);
        specialEnemyInflictorList.Add(specialSlowing);
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
 * File : OnePairTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 *
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 * 
 * Update : 2022/05/17 TUE
 * 타워의 특수 공격 구현.
 */