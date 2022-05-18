using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldEnemy : Enemy
{
    private Transform[] _wayPoints;  // �̵���� ��ǥ �迭
    private int _wayPointCount;     // �̵���� ����
    private int _currentIndex;   // ���� ��ǥ���� �ε���

    private float _baseMoveSpeed; // Enemy�� �̵� �ӵ� (���� �̻� ���Ǵ� �̵��ӵ�)
    private int _stunCount; // ������ ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ������ �˱� ���� ����
    private float _increaseReceiveDamageRate; // Enemy�� ���� ���� �� �޴� ���ط�

    private Movement2D _movement2D;  // ������Ʈ �̵� ����

    protected EnemySpawner enemySpawner { get; set; }

    protected virtual void Awake()
    {
        _movement2D = GetComponent<Movement2D>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public virtual void Setup(Transform[] wayPoints, EnemyData enemyData)
    {
        if(_wayPoints == null)
        {
            _wayPoints = wayPoints;
            _wayPointCount = wayPoints.Length;
        }

        // ������ Enemy�� ü�� ����
        maxHealth = enemyData.health;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        // ������ Enemy�� �̵��ӵ� ����
        _movement2D.moveSpeed = enemyData.moveSpeed;
        _baseMoveSpeed = enemyData.moveSpeed;

        _stunCount = 0;
        _increaseReceiveDamageRate = 0;

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
            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.05f���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.05f)
            {
                NextMoveTo();
                isNextMove = true;
            }

            // ���� ���� ��ǥ���� �Ÿ��� ���� ��ǥ���� �Ÿ����� ũ�ٸ� ��ǥ���κ��� �־����� �ִٴ� ���� �� �� �ִ�.
            // ��θ� ����� ������ enemy�� ��ġ�� ��ǥ ��ġ�� �ٽ� ����ش�.
            if (nowDistance > lastDistance)
            {
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
    }

    public override void TakeDamage(float damage)
    {
        damage *= 1f + (_increaseReceiveDamageRate * 0.01f);
        
        health -= damage;
        healthSlider.value -= damage;

        if (health <= 0)
            Die();
    }

    public override void TakeStun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }
    private IEnumerator StunCoroutine(float duration)
    {
        // stunCount 1 ����.
        _stunCount++;
        // �̵��ӵ��� 0���� ����.
        _movement2D.moveSpeed = 0f;

        // stunTime ��ŭ ����
        yield return new WaitForSeconds(duration);

        // ���� �ð��� ���� �Ǿ����Ƿ� stunCount 1 ����.
        _stunCount--;

        // ���� ���Ͽ� �ɸ��� ���� ���¶�� �̵��ӵ��� ������� �ǵ�����.
        if (_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;
    }


    public override void TakeSlowing(float slowingRate, float duration)
    {
        StartCoroutine(SlowingCoroutine(slowingRate, duration));
    }
    private IEnumerator SlowingCoroutine(float slowingRate, float duration)
    {
        // �����ϴ� �̵� �ӵ��� �����صд�.
        float slowSpeed = _baseMoveSpeed * slowingRate * 0.01f;

        // �����ϴ� �̵� �ӵ���ŭ ���ҽ�Ų��.
        _baseMoveSpeed -= slowSpeed;

        // ���� ������ �� Enemy�� ���� �̵� �ӵ��� �ٲٸ� ������ Ǯ���� �ȴ�.
        // ���� ���� ������ ��쿡�� �̵� �ӵ��� �ٲ��� �ʴ´�.
        if (_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;

        yield return new WaitForSeconds(duration);

        // ���ҽ��״� �̵� �ӵ��� �ǵ�����.
        _baseMoveSpeed += slowSpeed;
        // ���� ����.
        if (_stunCount == 0)
            _movement2D.moveSpeed = _baseMoveSpeed;
    }

    public override void TakeIncreaseReceivedDamage(float increaseReceivedDamageRate, float duration)
    {
        StartCoroutine(IncreaseReceivedDamageCoroutine(increaseReceivedDamageRate, duration));
    }

    private IEnumerator IncreaseReceivedDamageCoroutine(float increaseReceivedDamageRate, float duration)
    {
        _increaseReceiveDamageRate += increaseReceivedDamageRate;

        yield return new WaitForSeconds(duration);

        _increaseReceiveDamageRate -= increaseReceivedDamageRate;
    }

    protected abstract void OnMissing();
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
