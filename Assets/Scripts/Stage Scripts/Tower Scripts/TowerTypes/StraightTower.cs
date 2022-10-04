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
    private float[] _specialBuffRanges;
    [SerializeField]
    private Particle _buffRangeParticle;

    private BasicAttack _basicAttack;
    private Slowing _basicSlowing;
    private CriticalStrike _specialCriticalStrike;
    private IncreaseAttackRate _specialIncreaseAttackRate;

    private bool _isSpecialBuff;
    private float _specialBuffDuration => _specialIARateAttributes[level].duration;
    private float specialBuffRange => _specialBuffRanges[level];

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

        _basicAttack = new(this);
        baseEnemyInflictorList.Add(_basicAttack);

        _basicSlowing = new(this, _basicSlowingAttributes);
        baseEnemyInflictorList.Add(_basicSlowing);

        _specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(_specialCriticalStrike);

        _specialIncreaseAttackRate = new(this, _specialIARateAttributes);
        specialTowerInflictorList.Add(_specialIncreaseAttackRate);

        SetBuffRangeParticleScale();
    }

    protected override void RotateTower()
    {
        if (onTile != null)
            rotater2D.NaturalRotate();
    }

    public override void Setup()
    {
        base.Setup();
        SetBuffRangeParticleScale();

        _isSpecialBuff = false;
    }

    protected override void UpdateDetailInfo()
    {
        UpdateDetailInflictorInfo();

        detailBaseAttackInfo.Clear();
        detailBaseAttackInfo.Append(maxTargetCount.ToString());
        detailBaseAttackInfo.Append("명의 적을 공격");
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_basicAttack.inflictorInfo.ToString());
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_basicSlowing.inflictorInfo.ToString());

        detailSpecialAttackInfo.Clear();
        detailSpecialAttackInfo.Append(maxTargetCount.ToString());
        detailSpecialAttackInfo.Append("명의 적을 공격");
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append(_specialCriticalStrike.inflictorInfo.ToString());
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append("자신과 주변 타워 ");
        detailSpecialAttackInfo.Append(_specialIncreaseAttackRate.inflictorInfo.ToString());
    }

    protected override void AttackTarget()
    {
        if (!_isSpecialBuff) attackCount++;

        for (int i = 0; i < targetDetector.targetList.Count; i++)
        {
            if (attackCount < specialAttackCount)
                ShotProjectile(targetDetector.targetList[i], AttackType.Basic);
            else
                ShotProjectile(targetDetector.targetList[i], AttackType.Special);
        }

        if(attackCount < specialAttackCount)
            PlayAttackSound(AttackType.Basic);
        else
            PlayAttackSound(AttackType.Special);

        if (attackCount >= specialAttackCount)
        {
            _buffRangeParticle.PlayParticle();
            SpecialInflict(this, specialBuffRange);
            StartCoroutine(ToggleIsSpecialBuffCoroutine());
            attackCount = 0;

            SoundManager.instance.PlaySFX(SoundFileNameDictionary.rangeBuffSound);
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

    public override bool MergeTower(Tower mergeTower)
    {
        if (base.MergeTower(mergeTower))
        {
            SetBuffRangeParticleScale();
            return true;
        }

        return false;
    }

    private IEnumerator ToggleIsSpecialBuffCoroutine()
    {
        _isSpecialBuff = true;

        yield return new WaitForSeconds(_specialBuffDuration);

        _isSpecialBuff = false;
    }

    private void SetBuffRangeParticleScale()
    {
        float attackRangeScale = specialBuffRange * 2 / this.transform.lossyScale.x;
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