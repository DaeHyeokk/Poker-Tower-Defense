
using System.Collections;
using UnityEngine;

public class TopTower : Tower
{
    [Header("Basic Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _basicCritAttributes;

    [Header("Special Critical Strike")]
    [SerializeField]
    private CriticalStrike.Attribute[] _specialCritAttributes;

    [Header("Special Slowing")]
    [SerializeField]
    private Slowing.Attribute[] _specialSlowingAttributes;

    public override string towerName => "Top Tower";

    protected override void Awake()
    {
        base.Awake();
        targetDetector.detectingMode = TargetDetector.DetectingMode.Single;

        CriticalStrike basicCriticalStrike = new(this, _basicCritAttributes);
        basicInflictorList.Add(basicCriticalStrike);

        CriticalStrike specialCriticalStrike = new(this, _specialCritAttributes);
        specialInflictorList.Add(specialCriticalStrike);

        Slowing specialSlowing = new(this, _specialSlowingAttributes);
        specialInflictorList.Add(specialSlowing);
    }

    protected override void ShotProjectile(Enemy target, AttackType attackType)
    {
        if (attackType == AttackType.Basic)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => BasicInflict(projectile, target);
        }
        else // (attackType == AttackType.Special)
        {
            Projectile projectile = projectileSpawner.SpawnProjectile(this, spawnPoint, target, normalProjectileSprite);
            projectile.actionOnCollision += () => SpecialInflict(projectile, target);
        }
    }
}


/*
 * File : TopTower.cs
 * First Update : 2022/04/26 TUE 10:50
 * �߻�Ŭ���� TowerType�� ��ӹ޾� �߻�޼��带 �����ϴ� ������Ʈ
 * 
 * Update : 2022/04/30 SAT
 * TowerWeapon ���� �����丵�� �����ϸ鼭 ���� Tower���� Weapon���� �̸� ����
 */