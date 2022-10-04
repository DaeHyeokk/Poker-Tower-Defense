using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected SpriteRenderer _enemySprite;
    [SerializeField]
    private SpriteRenderer _healthbarGauge;
    [SerializeField]
    private SpriteRenderer _increaseReceiveDamageSprite;

    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ���
    protected int _rewardGold;
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ī�屳ȯ��
    protected int _rewardChangeChance;
    protected RewardStringBuilder _rewardStringBuilder;

    protected float _maxHealth;  // Enemy�� �ִ� ü��
    protected float _health;     // Enemy�� ���� ü��
    private float _increaseReceiveDamageRate; // Enemy�� ���� ���� �� �޴� ���ط�
    private IEnumerator _increaseReceivedDamageCoroutine;
    protected EnemySpawner _enemySpawner;
    protected EnemyHealthbar _enemyHealthbar;

    private readonly WaitForSeconds _takeDamageAnimationDelay = new(0.05f);

    protected float maxHealth => _maxHealth;  // Enemy�� �ִ� ü��
    protected float health => _health;  // Enemy�� ���� ü��
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


    public EnemySpawner enemySpawner => _enemySpawner;

    protected virtual void Awake()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _enemyHealthbar = new(_healthbarGauge);
        _rewardStringBuilder = new();
    }

    public virtual void Setup(EnemyData enemyData)
    {
        // ������ Enemy�� ��������Ʈ ���� �� ���� �ʱ�ȭ
        _enemySprite.sprite = enemyData.sprite;
        _enemySprite.color = Color.white;

        increaseReceiveDamageRate = 0f;
        StartCoroutine(SpawnAnimationCoroutine());
    }

    private IEnumerator SpawnAnimationCoroutine()
    {
        float lerpSpeed = 20f;
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime * lerpSpeed;
            float scale = Mathf.Lerp(0f, 1f, percent);
            this.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
    }

    public virtual void TakeDamage(Tower fromTower, float damage, DamageTakenType damageTakenType)
    {
        damage *= 1f + (increaseReceiveDamageRate * 0.01f);
        _health -= damage;
        _enemyHealthbar.health -= damage;

        StageUIManager.instance.ShowDamageTakenText(damage, this.transform, damageTakenType);

        if (_health <= 0)
            Die(fromTower);
        else
            StartCoroutine(EnemyTakeDamageAnimationCoroutine());
    }
    private IEnumerator EnemyTakeDamageAnimationCoroutine()
    {
        _enemySprite.color = Color.red;

        yield return _takeDamageAnimationDelay;

        _enemySprite.color = Color.white;
    }

    public void TakeIncreaseReceivedDamage(float increaseReceiveDamageRate, float duration)
    {
        // ���Ͱ� �̹� �޴� ���ط� ���� ������� �ް� ���� ��� ����.
        if (this.increaseReceiveDamageRate != 0f)
        {
            // �� ū ���� �޴� ���ط� ���� ������� ����ް� ���� ��� �ǳʶڴ�. 
            if (this.increaseReceiveDamageRate > increaseReceiveDamageRate)
                return;
            // �� ū ���� �޴� ���ط� ���� ������� �����ϴ� ���, ������ �ް� �ִ� ������� �����Ѵ�.
            else
                StopCoroutine(_increaseReceivedDamageCoroutine);
        }

        // ����� ����.
        _increaseReceivedDamageCoroutine = IncreaseReceivedDamageCoroutine(increaseReceiveDamageRate, duration);
        StartCoroutine(_increaseReceivedDamageCoroutine);
    }

    private IEnumerator IncreaseReceivedDamageCoroutine(float IRDRate, float duration)
    {
        this.increaseReceiveDamageRate = IRDRate;

        // duration��ŭ ����
        while (duration > 0)
        {
            yield return null;
            duration -= Time.deltaTime;
        }

        this.increaseReceiveDamageRate = 0f;
    }

    public abstract void TakeStun(float duration);
    public abstract void TakeSlowing(float slowingRate, float duration);

    protected virtual void Die(Tower fromTower)
    {
        // ������ ����� ���°� �ƴ� ���, FromTower�� ų�� ī��Ʈ ����.
        if(!StageManager.instance.isEnd)
            fromTower.AccumulateKillCount();

        ParticlePlayer.instance.PlayEnemyDie(_enemySprite.transform);
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.enemyDieSound);
        GiveReward();
    }

    protected virtual void GiveReward()
    {
        StageManager.instance.gold += _rewardGold;

        if (_rewardChangeChance > 0)
            StageManager.instance.changeChance += _rewardChangeChance;

        StageUIManager.instance.ShowEnemyDieRewardText(_rewardStringBuilder.ToString(), this.transform);
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
