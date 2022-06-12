using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTower : Tower
{
    [Header("Basic Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCritAttributes;

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Special Increase Damage Rate")]
    [SerializeField]
    private IncreaseDamageRate.Attribute[] _specialIncreaseDamageRateAttributes;

    [Header("Special Increase Attack Rate")]
    [SerializeField]
    private IncreaseAttackRate.Attribute[] _specialIncreaseAttackRateAttributes;

    private bool _isSpecialBuff;
    private readonly string _towerName = "트리플 타워";

    public override string towerName => _towerName;
    public override int towerIndex => 3;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        baseEnemyInflictorList.Add(basicCriticalStrike);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);

        IncreaseDamageRate specialIDRate = new(this, _specialIncreaseDamageRateAttributes);
        specialTowerInflictorList.Add(specialIDRate);

        IncreaseAttackRate specialIARate = new(this, _specialIncreaseAttackRateAttributes);
        specialTowerInflictorList.Add(specialIARate);
    }

    public override void Setup()
    {
        base.Setup();
        _isSpecialBuff = false;
    }

    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            // 공격할 타겟이 없다면 공격하지 않는다.
            if (targetDetector.targetList.Count == 0)
                yield return null;
            else
            {
                if(!_isSpecialBuff) attackCount++;

                for (int i = 0; i < targetDetector.targetList.Count; i++)
                {
                    if (attackCount < specialAttackCount)
                        ShotProjectile(targetDetector.targetList[i], AttackType.Basic);
                    else
                        ShotProjectile(targetDetector.targetList[i], AttackType.Special);
                }

                if (attackCount >= specialAttackCount)
                {
                    SpecialInflict(this);
                    attackCount = 0;
                }

                yield return attackDelay;
            }
        }
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
            projectile.actionOnCollision += () => SpecialInflict(target);
        }
    }
}


/*
 * File : TripleTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 * 
 * Update : 2022/05/17 TUE
 * 타워의 특수 공격 구현.
 */