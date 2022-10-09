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

    private float _specialBuffDuration => _specialIncreaseAttackRateAttributes[level].duration;

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
            StartCoroutine(ToggleIsSpecialBuffCoroutine());
            SpecialInflict(this);
            attackCount = 0;

            SoundManager.instance.PlaySFX(SoundFileNameDictionary.selfBuffSound);
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
 * File : TripleTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/17 TUE
 * Ÿ���� Ư�� ���� ����.
 */