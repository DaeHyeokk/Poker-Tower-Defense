using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTower : Tower
{
    [Header("Spawn Point Particle")]
    [SerializeField]
    private Particle _spawnPointParticle;

    [Header("Basic Increase Received Damage Rate")]
    [SerializeField]
    private IncreaseReceivedDamageRate.Attribute[] _basicIRDRateAttributes;

    [Header("Special Increase Damage Rate")]
    [SerializeField]
    private IncreaseDamageRate.Attribute[] _specialIDRateAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private float[] _specialBuffRanges;
    [SerializeField]
    private Particle _buffRangeParticle;

    private readonly string _towerName = "����ƾ Ÿ��";

    private float specialBuffRange => _specialBuffRanges[level];

    protected override int defaultSalesGold => 660;

    public override string towerName => _towerName;
    public override int towerIndex => 5;
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
        specialEnemyInflictorList.Add(basicAttack);

        IncreaseReceivedDamageRate basicIRDRate = new(this, _basicIRDRateAttributes);
        basicEnemyInflictorList.Add(basicIRDRate);

        IncreaseDamageRate specialIDRate = new(this, _specialIDRateAttributes);
        specialTowerInflictorList.Add(specialIDRate);

        SetBuffRangeParticleScale();
    }

    public override void Setup()
    {
        base.Setup();
        SetBuffRangeParticleScale();
    }

    protected override IEnumerator AttackTarget()
    {
        while (true)
        {
            // ������ Ÿ���� ���ٸ� �������� �ʴ´�.
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
                    SpecialInflict(this, specialBuffRange);

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
            projectile.actionOnCollision += () => BasicInflict(target);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
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

    private void SetBuffRangeParticleScale()
    {
        float attackRangeScale = specialBuffRange * 2 / this.transform.lossyScale.x;
        _buffRangeParticle.transform.localScale = new Vector3(attackRangeScale, attackRangeScale, 0);
    }
}


/*
 * File : MountainTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/17 TUE
 * Ÿ���� Ư�� ���� ����.
 */