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
    private float[] _specialBuffRanges;
    [SerializeField]
    private Particle _buffRangeParticle;

    private BasicAttack _basicAttack;
    private Slowing _basicSlowing;
    private CriticalStrike _specialCriticalStrike;
    private IncreaseAttackRate _specialIncreaseAttackRate;

    private readonly string _towerName = "��Ʈ����Ʈ Ÿ��";

    private float specialBuffRange => _specialBuffRanges[level];

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
    }
    protected override void UpdateDetailInfo()
    {
        UpdateDetailInflictorInfo();

        detailBaseAttackInfo.Clear();
        detailBaseAttackInfo.Append(maxTargetCount.ToString());
        detailBaseAttackInfo.Append("���� ���� ����");
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_basicAttack.inflictorInfo.ToString());
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_basicSlowing.inflictorInfo.ToString());

        detailSpecialAttackInfo.Clear();
        detailSpecialAttackInfo.Append("[���� ����]");
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append(maxTargetCount.ToString());
        detailSpecialAttackInfo.Append("���� ���� ����");
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append(_specialCriticalStrike.inflictorInfo.ToString());
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append("�ڽŰ� �ֺ� Ÿ�� ");
        detailSpecialAttackInfo.Append(_specialIncreaseAttackRate.inflictorInfo.ToString());
    }

    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            // ������ Ÿ���� ���ٸ� �������� �ʴ´�.
            if (targetDetector.targetList.Count == 0)
                yield return waitForFixedUpdate;
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
                    SpecialInflict(this, specialBuffRange);

                    attackCount = 0;
                }

                //yield return attackDelay;

                remainAttackDelay = attackRate;
                while (remainAttackDelay > 0)
                {
                    yield return waitForFixedUpdate;
                    remainAttackDelay -= Time.fixedDeltaTime;
                }
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
            projectile.actionOnCollision += () => SpecialInflict(target, _specialAttackRange);
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

    private void SetBuffRangeParticleScale()
    {
        float attackRangeScale = specialBuffRange * 2 / this.transform.lossyScale.x;
        _buffRangeParticle.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }
}


/*
 * File : StraightTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/17 TUE
 * Ÿ���� Ư�� ���� ����.
 */