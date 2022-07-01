using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldEnemy : Enemy
{
    [SerializeField]
    private Particle _slowEffect;
    [SerializeField]
    private Particle _stunEffect;

    private EnemyMovement _enemyMovement;
    protected GoldPenalty _goldPenalty;

    private int _stunCount; // 스턴을 중첩해서 맞을 경우 가장 마지막에 풀리는 스턴을 알기 위한 변수
    private int _slowCount; // 슬로우를 중첩해서 맞을 경우 가장 마지막에 풀리는 슬로우를 알기 위한 변수

    private int stunCount
    {
        get => _stunCount;
        set
        {
            // stunCount가 0이라면 스턴 파티클을 실행하고 이동을 멈춘다.
            if (_stunCount == 0 && value > 0)
            {
                _stunEffect.PlayParticle();
                _enemyMovement.Stop();
            }
            // stunCount가 0이 되면 스턴 파티클을 중지하고 이동을 재개한다.
            else if (_stunCount != 0 && value == 0)
            {
                _stunEffect.StopParticle();
                _enemyMovement.Move();
            }

            _stunCount = value;
        }
    }

    private int slowCount
    {
        get => _slowCount;
        set
        {
            // slowCount가 0이라면 슬로우 파티클을 실행한다.
            if (_slowCount == 0 && value > 0)
                _slowEffect.PlayParticle();
            // slowCount가 0이 되면 슬로우 파티클을 중지한다.
            else if (_slowCount != 0 && value == 0)
                _slowEffect.StopParticle();

            _slowCount = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _enemyMovement = GetComponent<EnemyMovement>();
        _goldPenalty = FindObjectOfType<WaveSystem>().goldPenalty;
        SetRewardText();

        GameManager.instance.OnGameEnd += GameoverAction;
    }

    public override void Setup(EnemyData enemyData)
    {
        base.Setup(enemyData);

        // 생성할 Enemy의 이동속도, 이동정보 설정
        _enemyMovement.moveSpeed = enemyData.moveSpeed;

        slowCount = 0;
        stunCount = 0;
    }

    public override void TakeStun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }
    private IEnumerator StunCoroutine(float duration)
    {
        // stunCount 1 증가.
        stunCount++;

        // duration 만큼 지연
        while (duration > 0)
        {
            //yield return _waitForFixedUpdate;
            yield return waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        // 스턴 시간이 종료 되었으므로 stunCount 1 감소.
        stunCount--;
    }


    public override void TakeSlowing(float slowingRate, float duration)
    {
        StartCoroutine(SlowingCoroutine(slowingRate, duration));
    }
    private IEnumerator SlowingCoroutine(float slowingRate, float duration)
    {
        // 감소하는 이동 속도를 저장해둔다.
        float slowSpeed = _enemyMovement.moveSpeed * slowingRate * 0.01f;
        // slowCount 1 증가.
        slowCount++;

        // 감소하는 이동 속도만큼 감소시킨다.
        _enemyMovement.moveSpeed -= slowSpeed;

        // duration 만큼 지연
        while (duration > 0)
        {
            //yield return _waitForFixedUpdate;
            yield return waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        // 감소시켰던 이동 속도를 되돌린다.
        _enemyMovement.moveSpeed += slowSpeed;

        // 증가시켰던 slowCount를 다시 감소시킨다.
        slowCount--;
    }

    protected override void Die()
    {
        base.Die();
        ReturnObject();
    }
    private void GameoverAction()
    {
        if (this.gameObject.activeSelf)
        {
            ParticlePlayer.instance.PlayEnemyDie(_enemySprite.transform);
            this.gameObject.SetActive(false);
        }
    }
    protected abstract void ReturnObject();
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
