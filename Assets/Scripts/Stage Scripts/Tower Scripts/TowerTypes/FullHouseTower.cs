using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHouseTower : Tower
{
    [Header("Basic CriticalStrike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCritAttributes;

    [Header("Special CriticalStrike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Special IncreaseDamageRate")]
    [SerializeField]
    private IncreaseDamageRate.Attribute[] _specialIncreaseDamageRateAttributes;

    [Header("Special IncreaseAttackRate")]
    [SerializeField]
    private IncreaseAttackRate.Attribute[] _specialIncreaseAttackRateAttributes;

    private bool _isSpecialBuff;

    private float _specialBuffDuration => _specialIncreaseAttackRateAttributes[level].duration;

    public override int towerIndex => 7;

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        baseEnemyInflictorList.Add(basicCriticalStrike);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);

        IncreaseDamageRate specialIncreaseDamageRate = new(this, _specialIncreaseDamageRateAttributes);
        specialTowerInflictorList.Add(specialIncreaseDamageRate);

        IncreaseAttackRate specialIncreaseAttackRate = new(this, _specialIncreaseAttackRateAttributes);
        specialTowerInflictorList.Add(specialIncreaseAttackRate);
    }

    public override void Setup()
    {
        base.Setup();
        _isSpecialBuff = false;
    }

    protected override void AttackTarget()
    {
        if (!_isSpecialBuff) attackCount++;

        for (int i = 0; i < targetDetector.targetList.Count; i++)
        {
            if (attackCount < specialAttackCount)
            {

                ShotProjectile(targetDetector.targetList[i], AttackType.Basic);
                PlayAttackSound(AttackType.Basic);
            }
            else
            {
                ShotProjectile(targetDetector.targetList[i], AttackType.Special);
                PlayAttackSound(AttackType.Special);
            }
        }

        if (attackCount >= specialAttackCount)
        {
            SpecialInflict(this);
            StartCoroutine(ToggleIsSpecialBuffCoroutine());
            attackCount = 0;

            SoundManager.instance.PlaySFX(SoundFileNameDictionary.selfBuffSound);
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

    private IEnumerator ToggleIsSpecialBuffCoroutine()
    {
        _isSpecialBuff = true;

        yield return new WaitForSeconds(_specialBuffDuration);

        _isSpecialBuff = false;
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
 * 
 * Update : 2022/05/27 FRI
 * Special Inflict 효과로 공격속도 증가 버프를 받고 있을 때는 적을 공격해도 Attack Count가 증가하지 않도록 변경.
 */
