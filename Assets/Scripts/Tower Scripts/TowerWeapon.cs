using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class TowerWeapon : MonoBehaviour
{
    public enum ColorType { Red, Green, Blue }
    public enum WeaponState { SearchTarget, AttackToTarget }

    [SerializeField]
    private GameObject _projectile;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private HorizontalLayoutGroup _gradeLayout;
    private EnemySpawner _enemySpawner;
    private Image[] _gradeImages;
    private Color[] _colorDatas;
    private SpriteRenderer _towerRenderer;
    private TowerData _towerData;
    private int _towerGrade;
    private Transform _attackTarget;
    private ColorType _towerColor;
    private WeaponState _weaponState = WeaponState.SearchTarget;

    public float attackDamage => _towerData.weapons[_towerGrade].damage;
    public float attackRate => _towerData.weapons[_towerGrade].rate;
    public float attackRange => _towerData.weapons[_towerGrade].range;
    public int towerGrade => _towerGrade;
    public ColorType towerColor => _towerColor;
    public WeaponState weaponState => _weaponState;
    public Transform attackTarget => _attackTarget;
    public abstract String towerName { get; }
    // public int killCount { get; private set; }// 미구현 : 유닛당 킬수 카운팅. 게임 종료 시 플레이어 데이터에 누적하는 용도

    private void Awake()
    {

        _gradeLayout.gameObject.SetActive(false);
        _gradeImages = new Image[3];
        _gradeImages = _gradeLayout.GetComponentsInChildren<Image>(true);
        _towerRenderer = GetComponentInChildren<SpriteRenderer>();

        _colorDatas = new Color[3];

        _colorDatas[0] = new Color(180, 0, 0);
        _colorDatas[1] = new Color(0, 180, 0);
        _colorDatas[2] = new Color(0, 0, 180);
    }

    // Tower의 능력치, 등급, 색상타입 세팅
    // 처음 생성되는 타워이므로 등급을 0으로 설정
    public void Setup(TowerData towerData, EnemySpawner enemySpawner)
    {
        _towerData = towerData;
        _towerGrade = 0;
        _attackTarget = null;
        _enemySpawner = enemySpawner;
        ColorSetup();
        ChangeState(WeaponState.SearchTarget);
    }

    // 타워의 상태를 매개변수 값으로 변경
    private void ChangeState(WeaponState newWeaponState)
    {
        // 이전에 실행중이던 상태 종료
        StopCoroutine(_weaponState.ToString());
        // 상태 변경
        _weaponState = newWeaponState;
        // 새로운 상태 실행
        StartCoroutine(_weaponState.ToString());
    }

    // Color Type은 Red, Green, Blue 셋 중 하나를 랜덤으로 설정함
    private void ColorSetup()
    {
        ColorType type = (ColorType)UnityEngine.Random.Range((int)ColorType.Red, (int)ColorType.Blue+1);
        _towerColor = type;

        _towerRenderer.color = _colorDatas[(int)towerColor];
    }

    private void Update()
    {
        if (attackTarget != null)
            ActionToTarget();
    }
    protected virtual void ActionToTarget()
    {
        // 원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극좌표계 이용
        // 각도 = arctan(y/x)
        // x, y 변위값 구하기
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // x, y 변위값을 바탕으로 각도 구하기
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        _towerRenderer.transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    protected virtual IEnumerator SearchTarget()
    {
        while( true )
        {
            float closestDistSqr = Mathf.Infinity;
            
            for(int i=0; i<_enemySpawner.enemyList.Count; i++)
            {
                float distance = Vector3.Distance(this.transform.position, _enemySpawner.enemyList[i].transform.position);

                if(distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    _attackTarget = _enemySpawner.enemyList[i].transform;
                }
            }

            if(_attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    protected virtual IEnumerator AttackToTarget()
    {
        while( true )
        {
            // target이 있는지 검사 (다른 발사체에 의해 제거 or Goal 지점까지 이동해 삭제됨 등)
            if(_attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // target이 공격범위 내에 있는지 검사 (공격범위를 벗어나게 되면 새로운 적을 탐색)
            float distance = Vector3.Distance(this.transform.position, attackTarget.transform.position);
            if(distance > attackRange)
            {
                _attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 공격속도 시간만큼 대기 후 공격
            yield return new WaitForSeconds(attackRate);

            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
    }
    // Tower의 등급을 업그레이드 하는 메서드, Tower의 최대 등급은 3이다
    public void GradeUp()
    {
        // Tower가 최대 등급에 도달했을 경우 작업을 수행하지 않는다
        if(_towerGrade >= 3)
        {
            return;
        }
        
        _towerGrade++;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        // 타워등급이 1이라면 => GradeUp()이 최초로 실행됐다면
        if (_towerGrade == 1)
            _gradeLayout.gameObject.SetActive(true);  // layoutGroup UI를 활성화 한다

        // 등급을 나타내는 이미지 한개를 활성화 시킨다
        _gradeImages[_towerGrade - 1].gameObject.SetActive(true);
    }
    public virtual void OnAttack(Enemy enemy)
    {    
        // 여기서 3은 게임 전역 데이터의 업그레이드 수치를 임의로 준 값
        enemy.OnDamage(_towerData.weapons[_towerGrade].damage + _towerData.weapons[_towerGrade].upgradeDIP * 3);
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
 */