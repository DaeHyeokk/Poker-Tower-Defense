using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private Transform[] _wayPoints;  // �̵���� ��ǥ �迭
    private int _wayPointCount;     // �̵���� ����
    private int _currentIndex;   // ���� ��ǥ���� �ε���

    private float _maxHealth;  // Enemy�� �ִ� ü��
    private float _health;     // Enemy�� ���� ü��

    private Movement2D _movement2D;  // ������Ʈ �̵� ����
    private Rotater2D _rotater2D;
    public event Action actionOnDeath;
    public event Action actionOnMissing;

    private void Awake()
    {
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
        _movement2D.setMoveSpeed(enemyData.moveSpeed);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        // ��������Ʈ �迭�� ù��° ���Һ��� Ž���ϱ� ���� currentIndex ���� 0���� �ٲ�
        _currentIndex = 0;
        // Enemy ���� ��ġ�� ù��° wayPoint ��ǥ�� ����
        transform.position = this._wayPoints[_currentIndex].position;
        // �� �̵�/��ǥ ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        float nowDistance = 0, lastDistance = Mathf.Infinity;
        bool isNextMove = false;
        while (true)
        {
            isNextMove = false;
            nowDistance = Vector3.Distance(_wayPoints[_currentIndex].position, this.transform.position);
            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.1���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            // 0.1f�� MoveSpeed�� �����ִ� ������ �̵� �ӵ��� ���� enemy�� ��� �� �����ӿ� 0.1f���� ���� �̵��� ���� �ֱ� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��ǥ ��θ� ���� enemy ������Ʈ�� �߻��� �� �ֱ� �����̴�.
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.05f * _movement2D.moveSpeed)
            {
                NextMoveTo();
                isNextMove = true;
            }

            // ���� ���� ��ǥ���� �Ÿ��� ���� ��ǥ���� �Ÿ����� ũ�ٸ� ��ǥ���κ��� �־����� �ִٴ� ���� �� �� �ִ�.
            // ��θ� ����� ������ enemy�� ��ġ�� ��ǥ ��ġ�� �ٽ� ����ش�.
            if (nowDistance > lastDistance)
                transform.position = _wayPoints[_currentIndex].position;

            if (isNextMove)
                lastDistance = Mathf.Infinity;
            else
                lastDistance = nowDistance;
            // 1������ ���
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // ���� ��ǥ ������ �����ִٸ�
        if (_currentIndex < _wayPointCount - 1)
        {
            // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = _wayPoints[_currentIndex].position;
            _currentIndex++;

            Vector3 direction = (_wayPoints[_currentIndex].position - transform.position).normalized;
            _movement2D.MoveTo(direction);
            _rotater2D.Rotate(direction);
        }
        // ���� ��ġ�� ������ ��ǥ �����̶��
        else
        {
            StopCoroutine("OnMove");
            if (actionOnMissing != null)
                actionOnMissing();
        }
    }

    public void OnDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    protected virtual void Die()
    {

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
 */
