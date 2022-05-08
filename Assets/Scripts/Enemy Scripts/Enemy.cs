using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider _healthSlider;

    private Transform[] _wayPoints;  // �̵���� ��ǥ �迭
    private int _wayPointCount;     // �̵���� ����
    private int _currentIndex;   // ���� ��ǥ���� �ε���

    private float _maxHealth;  // Enemy�� �ִ� ü��
    private float _health;     // Enemy�� ���� ü��
    private float _baseMoveSpeed; // Enemy�� �̵� �ӵ� (���� �̻� ���Ǵ� �̵��ӵ�)
    private int _stunCount; // ������ ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ������ �˱� ���� ����

    private EnemySpawner _enemySpawner;
    private Movement2D _movement2D;  // ������Ʈ �̵� ����
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

        // ������ Enemy�� ü�� �� �̵��ӵ� ����
        _maxHealth = enemyData.health;
        _health = _maxHealth;
        _movement2D.moveSpeed = enemyData.moveSpeed;
        _baseMoveSpeed = enemyData.moveSpeed;
        _stunCount = 0;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        // ��������Ʈ �迭�� ù��° ���Һ��� Ž���ϱ� ���� currentIndex ���� 0���� �ٲ�
        _currentIndex = 0;
        // Enemy ���� ��ġ�� ù��° wayPoint ��ǥ�� ����
        transform.position = this._wayPoints[_currentIndex].position;
        // �� �̵�/��ǥ ���� ���� �ڷ�ƾ �Լ� ����
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
            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.1f���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            // 0.1f�� MoveSpeed�� �����ִ� ������ �̵� �ӵ��� ���� enemy�� ��� �� �����ӿ� 0.1f���� ���� �̵��� ���� �ֱ� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��ǥ ��θ� ���� enemy ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.1f)
            {
                NextMoveTo();
                isNextMove = true;
            }

            // ���� ���� ��ǥ���� �Ÿ��� ���� ��ǥ���� �Ÿ����� ũ�ٸ� ��ǥ���κ��� �־����� �ִٴ� ���� �� �� �ִ�.
            // ��θ� ����� ������ enemy�� ��ġ�� ��ǥ ��ġ�� �ٽ� ����ش�.
            if (nowDistance > lastDistance)
            {
                Debug.Log("����缳��");
                lastDistance = Mathf.Infinity;
                transform.position = _wayPoints[_currentIndex].position;
            }

            if (isNextMove)
                lastDistance = Mathf.Infinity;
            else
            {
                lastDistance = nowDistance;
            }

            // 1������ ���
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
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
        // stunCount 1 ����.
        _stunCount++;
        // �̵��ӵ��� 0���� ����.
        _movement2D.moveSpeed = 0f;

        // stunTime ��ŭ ����
        yield return new WaitForSeconds(stunTime);

        // ���� �ð��� ���� �Ǿ����Ƿ� stunCount 1 ����.
        _stunCount--;

        // ���� ���Ͽ� �ɸ��� ���� ���¶�� �̵��ӵ��� ������� �ǵ�����.
        if (_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;
    }


    public void OnSlow(float slowPer, float slowTime)
    {
            StartCoroutine(OnSlowCoroutine(slowPer, slowTime));
    }
    private IEnumerator OnSlowCoroutine(float slowPer, float slowTime)
    {
        // �����ϴ� �̵� �ӵ��� �����صд�.
        float slowSpeed = _baseMoveSpeed * slowPer * 0.01f;

        // �����ϴ� �̵� �ӵ���ŭ ���ҽ�Ų��.
        _baseMoveSpeed -= slowSpeed;

        // ���� ������ �� Enemy�� ���� �̵� �ӵ��� �ٲٸ� ������ Ǯ���� �ȴ�.
        // ���� ���� ������ ��쿡�� �̵� �ӵ��� �ٲ��� �ʴ´�.
        if(_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;

        yield return new WaitForSeconds(slowTime);

        // ���ҽ��״� �̵� �ӵ��� �ǵ�����.
        _baseMoveSpeed += slowSpeed;
        // ���� ����.
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
