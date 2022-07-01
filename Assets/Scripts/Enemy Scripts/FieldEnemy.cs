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

    private int _stunCount; // ������ ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ������ �˱� ���� ����
    private int _slowCount; // ���ο츦 ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ���ο츦 �˱� ���� ����

    private int stunCount
    {
        get => _stunCount;
        set
        {
            // stunCount�� 0�̶�� ���� ��ƼŬ�� �����ϰ� �̵��� �����.
            if (_stunCount == 0 && value > 0)
            {
                _stunEffect.PlayParticle();
                _enemyMovement.Stop();
            }
            // stunCount�� 0�� �Ǹ� ���� ��ƼŬ�� �����ϰ� �̵��� �簳�Ѵ�.
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
            // slowCount�� 0�̶�� ���ο� ��ƼŬ�� �����Ѵ�.
            if (_slowCount == 0 && value > 0)
                _slowEffect.PlayParticle();
            // slowCount�� 0�� �Ǹ� ���ο� ��ƼŬ�� �����Ѵ�.
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

        // ������ Enemy�� �̵��ӵ�, �̵����� ����
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
        // stunCount 1 ����.
        stunCount++;

        // duration ��ŭ ����
        while (duration > 0)
        {
            //yield return _waitForFixedUpdate;
            yield return waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        // ���� �ð��� ���� �Ǿ����Ƿ� stunCount 1 ����.
        stunCount--;
    }


    public override void TakeSlowing(float slowingRate, float duration)
    {
        StartCoroutine(SlowingCoroutine(slowingRate, duration));
    }
    private IEnumerator SlowingCoroutine(float slowingRate, float duration)
    {
        // �����ϴ� �̵� �ӵ��� �����صд�.
        float slowSpeed = _enemyMovement.moveSpeed * slowingRate * 0.01f;
        // slowCount 1 ����.
        slowCount++;

        // �����ϴ� �̵� �ӵ���ŭ ���ҽ�Ų��.
        _enemyMovement.moveSpeed -= slowSpeed;

        // duration ��ŭ ����
        while (duration > 0)
        {
            //yield return _waitForFixedUpdate;
            yield return waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        // ���ҽ��״� �̵� �ӵ��� �ǵ�����.
        _enemyMovement.moveSpeed += slowSpeed;

        // �������״� slowCount�� �ٽ� ���ҽ�Ų��.
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
 * Enemy ������Ʈ�� ������ �� �̵��� Waypoint ��ǥ�� �����ϰ�,
 * ��ǥ ������ ������� �̵��ϸ� ������ WayPoint�� �������� �� ������Ʈ�� �ı��Ѵ�.
 * 
 * Update : 2022/04/21 THU 13:20
 * ������ƮǮ�� ����� �����Ͽ��� ������ ���� ���忡�� ����� ������Ʈ�� ����ؼ� �����ϰ� ��
 * ���� Setup() �޼��忡�� ������ WayPoints �迭�� �Ҵ�� ���� �ִ��� Ȯ���ϴ� ������ �߰��Ͽ� ���ʿ��� ������ ���� �ʵ��� �����Ͽ���,
 * WayPoints �迭�� ù��° �ε������� �ٽ� �̵��ϱ� ���� ������ ����ߴ� currentIndex ���� 0���� �ٲٴ� ������ �߰���
 * 
 * Update : 2022/04/22 FRI 02:25
 * maxHealth, health, enemyMovement.moveSpped ���� �߰��Ͽ���
 * Setup() �޼��忡�� �Ű������� �޴� EnemyData�� ���� �������� ���� �����鿡 ���� �Ҵ���
 * Enemy�� ��ǥ ������ ������ ��� ó���ؾ� �� ������ ��������Ʈ Action�� ���� �̷�������� ����
 * 
 * Update : 2022/04/28 THU 16:40
 * Enemy�� ȸ���� �� ª�Զ� ���� �ɸ��� ��� ���� ��θ� �������� ���ϰ� ��θ� ��Ż�ϴ� Enemy�� �߻��ϴ� ���װ� �߰ߵ�.
 * ���� enemy�� ��ǥ������ �Ÿ��� nowDistance, �� ������ �� enemy�� ��ǥ������ �Ÿ��� lastDistance ������ �����ϰ�, �� ���� �������ν�
 * nowDistance ���� lastDistance ������ Ŭ ��� ��ǥ���� �־����� �ִٰ� �Ǵ��ϰ� enemy�� position�� ��ǥ waypoint�� position������ �ٲ�
 * ��Ż�� ��θ� �ٽ� ����ִ� ������ �߰��Ͽ� �̸� �ذ��Ͽ���.
 * 
 * Update : 2022/05/01 SUN 22:10
 * �ʵ� ������ �������� ������ ��������Ʈ�� �����ϸ� �������� �������� �ٽ� ù��° ��������Ʈ�� ���� �̵��ϵ��� �����Ͽ���.
 * 
 * Update : 2022/05/09 MON 03:05
 * Enemy ��ü�� ���� �̻�(����, ���ο�) ����.
 */
