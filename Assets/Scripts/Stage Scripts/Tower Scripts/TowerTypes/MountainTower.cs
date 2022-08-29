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

    private BasicAttack _basicAttack;
    private IncreaseReceivedDamageRate _baseIRDRate;
    private IncreaseDamageRate _specialIDRate;


    private bool _isSpecialBuff;
    private float _specialBuffDuration => _specialIDRateAttributes[level].duration;
    private float specialBuffRange => _specialBuffRanges[level];

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

        _basicAttack = new(this);
        baseEnemyInflictorList.Add(_basicAttack);
        specialEnemyInflictorList.Add(_basicAttack);

        _baseIRDRate = new(this, _basicIRDRateAttributes);
        baseEnemyInflictorList.Add(_baseIRDRate);

        _specialIDRate = new(this, _specialIDRateAttributes);
        specialTowerInflictorList.Add(_specialIDRate);

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
        detailBaseAttackInfo.Append("���� ���� ����");
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_basicAttack.inflictorInfo.ToString());
        detailBaseAttackInfo.Append('\n');
        detailBaseAttackInfo.Append(_baseIRDRate.inflictorInfo.ToString());
        
        detailSpecialAttackInfo.Clear();
        detailSpecialAttackInfo.Append(maxTargetCount.ToString());
        detailSpecialAttackInfo.Append("���� ���� ����");
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append(_basicAttack.inflictorInfo.ToString());
        detailSpecialAttackInfo.Append('\n');
        detailSpecialAttackInfo.Append("�ڽŰ� �ֺ� Ÿ�� ");
        detailSpecialAttackInfo.Append(_specialIDRate.inflictorInfo.ToString());
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

        if (attackCount < specialAttackCount)
            PlayAttackSound(AttackType.Basic);
        else
            PlayAttackSound(AttackType.Special);

        if (attackCount >= specialAttackCount)
        {
            _buffRangeParticle.PlayParticle();
            SpecialInflict(this, specialBuffRange);
            StartCoroutine(ToggleIsSpecialBuffCoroutine());   
            attackCount = 0;

            SoundManager.instance.PlaySFX("Range Buff Sound");
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