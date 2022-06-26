using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldEnemy : Enemy
{
    [SerializeField]
    private Canvas _healthSliderCanvas;
    [SerializeField]
    private SpriteRenderer _increaseReceiveDamageSprite;
    [SerializeField]
    private Particle _slowEffect;
    [SerializeField]
    private Particle _stunEffect;

    private int _stunCount; // ������ ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ������ �˱� ���� ����
    private int _slowCount; // ���ο츦 ��ø�ؼ� ���� ��� ���� �������� Ǯ���� ���ο츦 �˱� ���� ����
    private float _increaseReceiveDamageRate; // Enemy�� ���� ���� �� �޴� ���ط�
    private EnemyMovementState _enemyMovementState = new();
    private WaitForFixedUpdate _waitForFixedUpdate = new();

    public EnemyMovementState enemyMovementState => _enemyMovementState;
    public EnemySpawner enemySpawner { get; set; }

    private int stunCount
    {
        get => _stunCount;
        set
        {
            // stunCount�� 0�̶�� ���� ��ƼŬ�� �����ϰ� �̵��� �����.
            if (_stunCount == 0 && value > 0)
            {
                _stunEffect.PlayParticle();
                _enemyMovementState.isStop = true;
            }
            // stunCount�� 0�� �Ǹ� ���� ��ƼŬ�� �����ϰ� �̵��� �簳�Ѵ�.
            else if (_stunCount != 0 && value == 0)
            {
                _stunEffect.StopParticle();
                _enemyMovementState.isStop = false;
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
        _healthSliderCanvas.worldCamera = Camera.main;
    }

    public virtual void Setup(EnemyData enemyData)
    {
        // ������ Enemy�� ü��, ���� ����
        maxHealth = enemyData.health;
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        enemySprite.sprite = enemyData.sprite;
        enemySprite.color = Color.white;

        // ������ Enemy�� �̵��ӵ� ����
        _enemyMovementState.Setup(enemyData.moveSpeed);

        slowCount = 0;
        stunCount = 0;
        increaseReceiveDamageRate = 0;

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
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

        // duration ��ŭ ����
        while (duration > 0)
        {
            yield return _waitForFixedUpdate;
            duration -= Time.fixedDeltaTime;
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
        float slowSpeed = _enemyMovementState.moveSpeed * slowingRate * 0.01f;
        // slowCount 1 ����.
        slowCount++;

        // �����ϴ� �̵� �ӵ���ŭ ���ҽ�Ų��.
        _enemyMovementState.moveSpeed -= slowSpeed;

        // duration ��ŭ ����
        while (duration > 0)
        {
            yield return _waitForFixedUpdate;
            duration -= Time.fixedDeltaTime;
        }

        // ���ҽ��״� �̵� �ӵ��� �ǵ�����.
        _enemyMovementState.moveSpeed += slowSpeed;

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

        // duration��ŭ ����
        while (duration > 0)
        {
            yield return _waitForFixedUpdate;
            duration -= Time.fixedDeltaTime;
        }

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
