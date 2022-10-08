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

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Inflict Range")]
    [SerializeField]
    private float[] _specialBuffRanges;
    [SerializeField]
    private Particle _buffRangeParticle;

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

        BasicAttack basicAttack = new(this);
        baseEnemyInflictorList.Add(basicAttack);

        IncreaseReceivedDamageRate basicIRDRate = new(this, _basicIRDRateAttributes);
        baseEnemyInflictorList.Add(basicIRDRate);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialEnemyInflictorList.Add(specialCriticalStrike);
        
        IncreaseDamageRate specialIDRate = new(this, _specialIDRateAttributes);
        specialTowerInflictorList.Add(specialIDRate);

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
        for (int i = 0; i < baseEnemyInflictorList.Count; i++)
        {
            detailBaseAttackInfo.Append(baseEnemyInflictorList[i].inflictorInfo.ToString());
            detailBaseAttackInfo.Append('\n');
        }
        for (int i = 0; i < baseTowerInflictorList.Count; i++)
        {
            detailBaseAttackInfo.Append(baseTowerInflictorList[i].inflictorInfo.ToString());
            detailBaseAttackInfo.Append('\n');
        }

        detailSpecialAttackInfo.Clear();
        detailSpecialAttackInfo.Append(maxTargetCount.ToString());
        detailSpecialAttackInfo.Append("���� ���� ����");
        detailSpecialAttackInfo.Append('\n');
        for (int i = 0; i < specialEnemyInflictorList.Count; i++)
        {
            detailSpecialAttackInfo.Append(specialEnemyInflictorList[i].inflictorInfo.ToString());
            detailSpecialAttackInfo.Append('\n');
        }
        detailSpecialAttackInfo.Append("�ڽŰ� �ֺ� Ÿ�� ");
        for (int i = 0; i < specialTowerInflictorList.Count; i++)
        {
            detailSpecialAttackInfo.Append(specialTowerInflictorList[i].inflictorInfo.ToString());
            detailSpecialAttackInfo.Append('\n');
        }
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

            SoundManager.instance.PlaySFX(SoundFileNameDictionary.rangeBuffSound);
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