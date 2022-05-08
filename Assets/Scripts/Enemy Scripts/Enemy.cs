using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider _healthSlider;

    private Transform[] _wayPoints;  // 이동경로 좌표 배열
    private int _wayPointCount;     // 이동경로 개수
    private int _currentIndex;   // 현재 목표지점 인덱스

    private float _maxHealth;  // Enemy의 최대 체력
    private float _health;     // Enemy의 현재 체력
    private float _baseMoveSpeed; // Enemy의 이동 속도 (상태 이상에 사용되는 이동속도)
    private int _stunCount; // 스턴을 중첩해서 맞을 경우 가장 마지막에 풀리는 스턴을 알기 위한 변수

    private EnemySpawner _enemySpawner;
    private Movement2D _movement2D;  // 오브젝트 이동 제어
    private Rotater2D _rotater2D;

    private void Awake()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _movement2D = GetComponent<Movement2D>();
        _rotater2D = GetComponent<Rotater2D>();
    }

    public void Setup(Transform[] wayPoints, EnemyData enemyData)
    {
        if(_wayPoints == null)
        {
            _wayPoints = wayPoints;
            _wayPointCount = _wayPoints.Length;
        }

        // 생성할 Enemy의 체력 및 이동속도 설정
        _maxHealth = enemyData.health;
        _health = _maxHealth;
        _movement2D.moveSpeed = enemyData.moveSpeed;
        _baseMoveSpeed = enemyData.moveSpeed;
        _stunCount = 0;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        // 웨이포인트 배열의 첫번째 원소부터 탐색하기 위해 currentIndex 값을 0으로 바꿈
        _currentIndex = 0;
        // Enemy 등장 위치를 첫번째 wayPoint 좌표로 설정
        transform.position = this._wayPoints[_currentIndex].position;
        // 적 이동/목표 지점 설정 코루틴 함수 시작
        StartCoroutine(OnMoveCroutine());
    }

    private IEnumerator OnMoveCroutine()
    {
        NextMoveTo();

        float nowDistance = 0, lastDistance = Mathf.Infinity;
        bool isNextMove = false;
        while (true)
        {
            isNextMove = false;
            nowDistance = Vector3.Distance(this.transform.position, _wayPoints[_currentIndex].position);
            // 현재 enemy의 위치와 목표 지점의 위치 사이의 거리가 0.1f보다 가깝다면 다음 목표 지점을 탐색한다
            // 0.1f에 MoveSpeed를 곱해주는 이유는 이동 속도가 빠른 enemy일 경우 한 프레임에 0.1f보다 많이 이동할 수도 있기 때문에
            // if 조건문에 걸리지 않고 목표 경로를 잃은 enemy 오브젝트가 발생할 수 있기 때문이다.
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.1f)
            {
                NextMoveTo();
                isNextMove = true;
            }

            // 만약 현재 목표와의 거리가 이전 목표와의 거리보다 크다면 목표물로부터 멀어지고 있다는 것을 알 수 있다.
            // 경로를 벗어났기 때문에 enemy의 위치를 목표 위치로 다시 잡아준다.
            if (nowDistance > lastDistance)
            {
                Debug.Log("경로재설정");
                lastDistance = Mathf.Infinity;
                transform.position = _wayPoints[_currentIndex].position;
            }

            if (isNextMove)
                lastDistance = Mathf.Infinity;
            else
            {
                lastDistance = nowDistance;
            }

            // 1프레임 대기
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // Enemy의 위치를 정확하게 목표 위치로 설정
        transform.position = _wayPoints[_currentIndex].position;
        _currentIndex++;

        if (_currentIndex >= _wayPointCount)
            _currentIndex = 0;

        Vector3 direction = (_wayPoints[_currentIndex].position - transform.position).normalized;
        _movement2D.MoveTo(direction);
        _rotater2D.EnemyRotate(direction);
    }

    public void OnDamage(float damage)
    {
        _health -= damage;
        _healthSlider.value = _health;

        if (_health <= 0)
            Die();
    }

    public void OnStun(float stunTime)
    {
        StartCoroutine(OnStunCoroutine(stunTime));
    }
    private IEnumerator OnStunCoroutine(float stunTime)
    {
        // stunCount 1 증가.
        _stunCount++;
        // 이동속도를 0으로 변경.
        _movement2D.moveSpeed = 0f;

        // stunTime 만큼 지연
        yield return new WaitForSeconds(stunTime);

        // 스턴 시간이 종료 되었으므로 stunCount 1 감소.
        _stunCount--;

        // 만약 스턴에 걸리지 않은 상태라면 이동속도를 원래대로 되돌린다.
        if (_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;
    }


    public void OnSlow(float slowPer, float slowTime)
    {
            StartCoroutine(OnSlowCoroutine(slowPer, slowTime));
    }
    private IEnumerator OnSlowCoroutine(float slowPer, float slowTime)
    {
        // 감소하는 이동 속도를 저장해둔다.
        float slowSpeed = _baseMoveSpeed * slowPer * 0.01f;

        // 감소하는 이동 속도만큼 감소시킨다.
        _baseMoveSpeed -= slowSpeed;

        // 스턴 상태일 때 Enemy의 실제 이동 속도를 바꾸면 스턴이 풀리게 된다.
        // 따라서 스턴 상태일 경우에는 이동 속도를 바꾸지 않는다.
        if(_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;

        yield return new WaitForSeconds(slowTime);

        // 감소시켰던 이동 속도를 되돌린다.
        _baseMoveSpeed += slowSpeed;
        // 위와 동일.
        if(_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;
    }

    protected virtual void Die()
    {
        _enemySpawner.enemyList.Remove(this);
        ReturnPool();
    }

    private void ReturnPool()
    {
        _enemySpawner.enemyPool.ReturnObject(this);
    }
}

/*
 * File : Enemy.cs
 * First Update : 2022/04/20 WED 14:57
 * Enemy 오브젝트가 생성될 때 이동할 Waypoint 좌표를 설정하고,
 * 목표 지점을 순서대로 이동하며 마지막 WayPoint에 도착했을 시 오브젝트를 파괴한다.
 * 
 * Update : 2022/04/21 THU 13:20
 * 오브젝트풀링 기법을 적용하였기 때문에 이전 라운드에서 사용한 오브젝트를 계속해서 재사용하게 됨
 * 따라서 Setup() 메서드에서 이전에 WayPoints 배열에 할당된 값이 있는지 확인하는 로직을 추가하여 불필요한 연산을 하지 않도록 변경하였고,
 * WayPoints 배열의 첫번째 인덱스부터 다시 이동하기 위해 이전에 사용했던 currentIndex 값을 0으로 바꾸는 로직을 추가함
 * 
 * Update : 2022/04/22 FRI 02:25
 * maxHealth, health, enemyMovement.moveSpped 값을 추가하였음
 * Setup() 메서드에서 매개변수로 받는 EnemyData의 값을 바탕으로 위의 변수들에 값을 할당함
 * Enemy가 목표 지점에 도달할 경우 처리해야 할 연산이 델리게이트 Action를 통해 이루어지도록 구현
 * 
 * Update : 2022/04/28 THU 16:40
 * Enemy가 회전할 때 짧게라도 렉이 걸리는 경우 다음 경로를 배정받지 못하고 경로를 이탈하는 Enemy가 발생하는 버그가 발견됨.
 * 현재 enemy와 목표물과의 거리를 nowDistance, 한 프레임 전 enemy와 목표물과의 거리를 lastDistance 변수에 저장하고, 그 값을 비교함으로써
 * nowDistance 값이 lastDistance 값보다 클 경우 목표물과 멀어지고 있다고 판단하고 enemy의 position을 목표 waypoint의 position값으로 바꿔
 * 이탈한 경로를 다시 잡아주는 로직을 추가하여 이를 해결하였다.
 * 
 * Update : 2022/05/01 SUN 22:10
 * 필드 디자인 변경으로 마지막 웨이포인트에 도달하면 없어지는 로직에서 다시 첫번째 웨이포인트를 향해 이동하도록 변경하였음.
 * 
 * Update : 2022/05/09 MON 03:05
 * Enemy 개체의 상태 이상(스턴, 슬로우) 구현.
 */
