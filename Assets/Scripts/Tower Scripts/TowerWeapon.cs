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
    // public int killCount { get; private set; }// 미구현 : 유닛당 킬수 카운팅. 게임 종료 시 플레이어 데이터에 누적하는 용도

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
                // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극좌표계 이용
                // 각도 = arctan(y/x)
                // x, y 변위값 구하기
                float dx = _targetDetector.enemyTarget.transform.position.x - _targetDetector.transform.position.x;
                float dy = _targetDetector.enemyTarget.transform.position.y - _targetDetector.transform.position.y;
                // x, y 변위값을 바탕으로 각도 구하기
                // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
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
            // target이 있는지 검사 (다른 발사체에 의해 제거 or Goal 지점까지 이동해 삭제됨 등)
            if (_targetDetector.enemyTarget != null)
            {
                // 공격속도 시간만큼 대기 후 공격
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
        // 여기서 3은 게임 전역 데이터의 업그레이드 수치를 임의로 준 값
       // enemy.OnDamage(_weaponData.weapons[_towerGrade].damage + _weaponData.weapons[_towerGrade].upgradeDIP * 3);
    }

    // OnSkill() 메서드는 Weapon Type마다 다르게 동작해야 하기 때문에
    // 자식클래스에서 OnSkill() 메서드를 직접 구현하도록 강제하기 위해 Abstract Method로 선언
    public abstract void OnSkill();

}

/*
 * File : TowerWeapon.cs
 * First Update : 2022/04/25 MON 10:52
 * 타워의 공격을 담당하는 스크립트.
 * 타워의 종류, 공격력, 공격속도 등 공격에 관련된 데이터를 가지고 있으며,
 * 타워 종류마다 각각 다른 스킬을 구현해야 하기 때문에 추상 클래스로 선언하였다.
 * TowerWeapon 클래스를 상속받는 9개의 서브클래스가 있으며 서브클래스에서 OnSkill() 메서드를 구현한다.
 * 
 * Update : 2022/04/28 THU 23:30
 * 맵에 있는 Enemy 들의 정보를 가져와 사거리 내에 있는 적을 탐색하는 기능 구현.
 * 목표물을 향해 공격속도 시간마다 발사체를 발사하는 기능 구현.
 * 
 * Update : 2022/04/30 SAT 23:15
 * 타워 구조를 재설계하며 기존에 TowerWeapon에서 담당하던 기능들을 세분화하여 컴포넌트로 만듬.
 * TargetDetector 컴포넌트에서 찾은 Target을 참조하여 해당 적을 AttackRate 시간마다 공격하도록 변경.
 */