using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTower : Tower
{
    [Header("Spawn Point Particle")]
    [SerializeField]
    private Particle _spawnPointParticle;

    [Header("Basic Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _basicSlowingAttributes;

    [Header("Special Increase Attack Rate")]
    [SerializeField]
    private IncreaseAttackRate.Attribute[] _specialIARateAttributes;

    [Header("Special CriticalStrike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private float _specialAttackRange;
    [SerializeField]
    private float _specialBuffRange;
    [SerializeField]
    private Particle _buffRangeParticle;

    private readonly string _towerName = "Straight Tower";

    public override string towerName => _towerName;
    public override int towerIndex => 4;
    public override Tile onTile
    {
        get => base.onTile;
        set
        {
            if (value == null)
            {
                base.onTile = null;
                _spawnPointParticle.StopParticle();
            }
            else
            {
                base.onTile = value;
                _spawnPointParticle.PlayParticle();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Multiple;

        BasicAttack basicAttack = new(this);
        basicEnemyInflictorList.Add(basicAttack);

        Slowing basicSlowing = new(this, _basicSlowingAttributes);
        basicEnemyInflictorList.Add(basicSlowing);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);

        IncreaseAttackRate specialIncreaseAttackRate = new(this, _specialIARateAttributes);
        specialTowerInflictorList.Add(specialIncreaseAttackRate);

        SetBuffRangeParticleScale();
    }

    protected override IEnumerator SearchAndAction()
    {
        while (true)
        {
            // 타일 위에 배치된 상태가 아니라면 적을 탐색하지 않는다.
            if (onTile == null)
                yield return null;

            targetDetector.SearchTarget();

            // 공격할 타겟이 없다면 공격하지 않는다.
            if (targetDetector.targetList.Count == 0)
                yield return null;
            else
            {
                attackCount++;

                for (int i = 0; i < targetDetector.targetList.Count; i++)
                {
                    if (attackCount < specialAttackCount)
                        ShotProjectile(targetDetector.targetList[i], AttackType.Basic);
                    else
                        ShotProjectile(targetDetector.targetList[i], AttackType.Special);
                }

                if (attackCount >= specialAttackCount)
                {
                    _buffRangeParticle.PlayParticle();
                    SpecialInflict(this, _specialBuffRange);

                    attackCount = 0;
                }

                yield return new WaitForSeconds(attackRate);
            }
        }
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(target);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, specialProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(target, _specialAttackRange);
        }
    }

    private void SetBuffRangeParticleScale()
    {
        float attackRangeScale = _specialBuffRange * 2 / this.transform.lossyScale.x;
        _buffRangeParticle.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }
}


/*
 * File : StraightTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * 추상클래스 TowerType을 상속받아 추상메서드를 구현하는 컴포넌트
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon 관련 리팩토링을 진행하면서 기존 Tower에서 Weapon으로 이름 변경
 * 
 * Update : 2022/05/17 TUE
 * 타워의 특수 공격 구현.
 */