using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class TowerWeapon : MonoBehaviour
{
    public enum WeaponState { SearchTarget, AttackToTarget }

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private WeaponData _weaponData;

    private Transform _spawnPoint;
    private SpriteRenderer _towerRenderer;
    private TowerLevel _towerLevel;
    private TargetDetector _targetDetector;
    private WaitForSeconds _attackRateDelay;

    public float damage => _weaponData.weapons[_towerLevel.levelCount].damage;
    public float attackRate => _weaponData.weapons[_towerLevel.levelCount].rate;
    public float range => _weaponData.weapons[_towerLevel.levelCount].range;

    public abstract String weaponName { get; }
    // public int killCount { get; private set; }// �̱��� : ���ִ� ų�� ī����. ���� ���� �� �÷��̾� �����Ϳ� �����ϴ� �뵵

    private void Awake()
    {
        _spawnPoint = GetComponentInChildren<Transform>();
        _towerRenderer = GetComponent<SpriteRenderer>();
    }

    public void DefaultSetup()
    {
        _towerLevel = GetComponentInParent<TowerLevel>();
        _targetDetector = GetComponentInParent<TargetDetector>();

        _attackRateDelay = new WaitForSeconds(attackRate);
    }

    public virtual void StartActionToTarget()
    {
        StartCoroutine(RotateToTarget());
        StartCoroutine(AttackToTarget());
    }

    public virtual void StopActionToTarget()
    {
        StopCoroutine(RotateToTarget());
        StopCoroutine(AttackToTarget());
    }

    public IEnumerator RotateToTarget()
    {
        while(true)
        {
            if (_targetDetector.enemyTarget != null)
            {
                // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� ����ǥ�� �̿�
                // ���� = arctan(y/x)
                // x, y ������ ���ϱ�
                float dx = _targetDetector.enemyTarget.transform.position.x - _targetDetector.transform.position.x;
                float dy = _targetDetector.enemyTarget.transform.position.y - _targetDetector.transform.position.y;
                // x, y �������� �������� ���� ���ϱ�
                // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
                float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
                _towerRenderer.transform.rotation = Quaternion.Euler(0, 0, degree);
            }

            yield return null;
        }
    }

    protected virtual IEnumerator AttackToTarget()
    {
        while( true )
        {
            // target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ���� or Goal �������� �̵��� ������ ��)
            if (_targetDetector.enemyTarget != null)
            {
                // ���ݼӵ� �ð���ŭ ��� �� ����
                yield return _attackRateDelay;

                if (_targetDetector.enemyTarget != null)
                    SpawnProjectile();
            }

            yield return null;
        }
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(_targetDetector.enemyTarget.transform);
    }


    public virtual void OnAttack(Enemy enemy)
    {    
        // ���⼭ 3�� ���� ���� �������� ���׷��̵� ��ġ�� ���Ƿ� �� ��
       // enemy.OnDamage(_weaponData.weapons[_towerGrade].damage + _weaponData.weapons[_towerGrade].upgradeDIP * 3);
    }

    // OnSkill() �޼���� Weapon Type���� �ٸ��� �����ؾ� �ϱ� ������
    // �ڽ�Ŭ�������� OnSkill() �޼��带 ���� �����ϵ��� �����ϱ� ���� Abstract Method�� ����
    public abstract void OnSkill();

}

/*
 * File : TowerWeapon.cs
 * First Update : 2022/04/25 MON 10:52
 * Ÿ���� ������ ����ϴ� ��ũ��Ʈ.
 * Ÿ���� ����, ���ݷ�, ���ݼӵ� �� ���ݿ� ���õ� �����͸� ������ ������,
 * Ÿ�� �������� ���� �ٸ� ��ų�� �����ؾ� �ϱ� ������ �߻� Ŭ������ �����Ͽ���.
 * TowerWeapon Ŭ������ ��ӹ޴� 9���� ����Ŭ������ ������ ����Ŭ�������� OnSkill() �޼��带 �����Ѵ�.
 * 
 * Update : 2022/04/28 THU 23:30
 * �ʿ� �ִ� Enemy ���� ������ ������ ��Ÿ� ���� �ִ� ���� Ž���ϴ� ��� ����.
 * ��ǥ���� ���� ���ݼӵ� �ð����� �߻�ü�� �߻��ϴ� ��� ����.
 * 
 * Update : 2022/04/30 SAT 23:15
 * Ÿ�� ������ �缳���ϸ� ������ TowerWeapon���� ����ϴ� ��ɵ��� ����ȭ�Ͽ� ������Ʈ�� ����.
 * TargetDetector ������Ʈ���� ã�� Target�� �����Ͽ� �ش� ���� AttackRate �ð����� �����ϵ��� ����.
 */