using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldEnemy : Enemy
{
    [SerializeField]
    private SpriteRenderer _increaseReceiveDamageSprite;
    [SerializeField]
    private Particle _slowEffect;
    [SerializeField]
    private Particle _stunEffect;

    private Transform[] _wayPoints;  // �̵���� ��ǥ �迭
    private int _currentIndex;   // ���� ��ǥ���� �ε���

    private int _stunCount; // ������ ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ������ �˱� ���� ����
    private int _slowCount; // ���ο츦 ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ���ο츦 �˱� ���� ����
    private float _increaseReceiveDamageRate; // Enemy�� ���� ���� �� �޴� ���ط�

    private Movement2D _movement2D;  // ������Ʈ �̵� ����

    private int stunCount
    {
        get => _stunCount;
        set
        {
            // stunCount�� 0�̶�� ���� ��ƼŬ�� �����ϰ� �̵��� �����.
            if (_stunCount == 0 && value > 0)
            {
                _stunEffect.PlayParticle();
                _movement2D.Stop();
            }
            // stunCount�� 0�� �Ǹ� ���� ��ƼŬ�� �����ϰ� �̵��� �簳�Ѵ�.
            else if (_stunCount != 0 && value == 0)
            {
                _stunEffect.StopParticle();
                _movement2D.Move();
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

    private float increaseReceiveDamageRate
    {
        get => _increaseReceiveDamageRate;
        set
        {
            // increaseReceiveDamageRate ���� 0�̰� value���� 0���� ũ�ٸ� ����� ��������Ʈ�� Ȱ��ȭ �Ѵ�.
            if (_increaseReceiveDamageRate == 0 && value > 0)
                _increaseReceiveDamageSprite.gameObject.SetActive(true);
            // increaseReceiveDamageRate ���� 0�� �ƴϰ� value���� 0�̶�� ����� ��������Ʈ�� ��Ȱ��ȭ �Ѵ�.
            else if (_increaseReceiveDamageRate > 0 && value == 0)
                _increaseReceiveDamageSprite.gameObject.SetActive(false);

            _increaseReceiveDamageRate = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _movement2D = GetComponent<Movement2D>();
    }

    public virtual void Setup(Transform[] wayPoints, EnemyData enemyData)
    {
        if(_wayPoints == null)
            _wayPoints = wayPoints;

        // ������ Enemy�� ü��, ���� ����
        maxHealth = enemyData.health;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        enemySprite.sprite = enemyData.sprite;
        enemySprite.color = Color.white;

        // ������ Enemy�� �̵��ӵ� ����
        _movement2D.moveSpeed = enemyData.moveSpeed;

        slowCount = 0;
        stunCount = 0;
        increaseReceiveDamageRate = 0;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        // ��������Ʈ �迭�� ù��° ���Һ��� Ž���ϱ� ���� currentIndex ���� 0���� �ٲ�
        _currentIndex = 0;
        // Enemy ���� ��ġ�� ù��° wayPoint ��ǥ�� ����
        transform.position = this._wayPoints[_currentIndex].position;
        // �� �̵�/��ǥ ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine(OnMoveCoroutine());
    }

    private IEnumerator OnMoveCoroutine()
    {
        NextMoveTo();

        float nowDistance = 0, lastDistance = Mathf.Infinity;
        bool isNextMove = false;
        while (true)
        {
            isNextMove = false;
            nowDistance = Vector3.Distance(this.transform.position, _wayPoints[_currentIndex].position);

            // ���� enemy�� ��ġ�� ��ǥ ������ ��ġ ������ �Ÿ��� 0.05f���� �����ٸ� ���� ��ǥ ������ Ž���Ѵ�
            // ���� ���� ��ǥ���� �Ÿ��� ���� ��ǥ���� �Ÿ����� ũ�ٸ� ��ǥ���κ��� �־����� �ִٴ� ���� �� �� �ִ�.
            // ��θ� ����� ������ ���� ��ǥ ������ Ž���Ѵ�.
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.05f || nowDistance > lastDistance)
            {
                NextMoveTo();
                isNextMove = true;
            }

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
        // Enemy�� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
        transform.position = _wayPoints[_currentIndex].position;
        _currentIndex++;

        if (_currentIndex >= _wayPoints.Length)
            _currentIndex = 0;

        Vector3 direction = (_wayPoints[_currentIndex].position - transform.position).normalized;
        _movement2D.MoveTo(direction);
    }

    public override void TakeDamage(float damage, DamageTakenType damageTakenType)
    {
        base.TakeDamage(damage, damageTakenType);

        damage *= 1f + (_increaseReceiveDamageRate * 0.01f);

        health -= damage;
        healthSlider.value -= damage;

        UIManager.instance.ShowDamageTakenText(damage, this.transform, damageTakenType);

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
        stunCount++;

        // stunTime ��ŭ ����
        yield return new WaitForSeconds(duration);

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
        float slowSpeed = _movement2D.moveSpeed * slowingRate * 0.01f;

        // slowCount 1 ����.
        slowCount++;

        // �����ϴ� �̵� �ӵ���ŭ ���ҽ�Ų��.
        _movement2D.moveSpeed -= slowSpeed;

        yield return new WaitForSeconds(duration);

        // ���ҽ��״� �̵� �ӵ��� �ǵ�����.
        _movement2D.moveSpeed += slowSpeed;

        // �������״� slowCount�� �ٽ� ���ҽ�Ų��.
        slowCount--;
    }

    public override void TakeIncreaseReceivedDamage(float increaseReceivedDamageRate, float duration)
    {
        StartCoroutine(IncreaseReceivedDamageCoroutine(increaseReceivedDamageRate, duration));
    }

    private IEnumerator IncreaseReceivedDamageCoroutine(float increaseReceivedDamageRate, float duration)
    {
        this.increaseReceiveDamageRate += increaseReceivedDamageRate;

        yield return new WaitForSeconds(duration);

        this.increaseReceiveDamageRate -= increaseReceivedDamageRate;
    }

    protected override void Die()
    {
        base.Die();
        ReturnObject();
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
