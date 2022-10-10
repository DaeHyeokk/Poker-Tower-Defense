using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourKindTower : Tower
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

    public override int towerIndex => 8;

    private bool _isSpecialBuff;

    private float _specialBuffDuration => _specialIncreaseAttackRateAttributes[level].duration;

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

    private IEnumerator ToggleIsSpecialBuffCoroutine()
    {
        _isSpecialBuff = true;

        float specialBuffDuration = _specialBuffDuration;
        while (specialBuffDuration > 0f)
        {
            specialBuffDuration -= Time.deltaTime;
            yield return null;
        }

        _isSpecialBuff = false;
    }
}


/*
 * File : FourKindTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 * 
 * Update : 2022/05/16 MON
 * Ÿ���� Ư�� ���� ����.
 */