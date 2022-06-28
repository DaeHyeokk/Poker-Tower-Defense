using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _enemySprite;
    [SerializeField]
    private Transform _healthbarGauge;
    [SerializeField]
    private SpriteRenderer _increaseReceiveDamageSprite;
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ���
    [SerializeField]
    private int _rewardGold;
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ī�屳ȯ��
    [SerializeField]
    private int _rewardChangeChance;
    // Enemy�� ���� ��� �÷��̾�� ���޵Ǵ� ��Ŀī��
    [SerializeField]
    private int _rewardJokerCard;

    private EnemySpawner _enemySpawner;
    private EnemyHealthbar _enemyHealthbar;
    private StringBuilder _rewardText = new();
    private float _maxHealth;  // Enemy�� �ִ� ü��
    private float _health;     // Enemy�� ���� ü��
    private float _increaseReceiveDamageRate; // Enemy�� ���� ���� �� �޴� ���ط�

    private readonly WaitForSeconds _takeDamageAnimationDelay = new(0.05f);
    private readonly WaitForSeconds _waitForPointFiveSeconds = new(0.5f);

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

    protected WaitForSeconds waitForPointFiveSeconds => _waitForPointFiveSeconds;

    public EnemySpawner enemySpawner => _enemySpawner;

    protected virtual void Awake()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _enemyHealthbar = new(_healthbarGauge);
        SetRewardText();
    }

    public virtual void Setup(EnemyData enemyData)
    {
        // ������ Enemy�� ü��, ���� ����
        _maxHealth = enemyData.health;
        _health = _maxHealth;
        _enemyHealthbar.maxHealth = _maxHealth;
        _enemyHealthbar.health = _maxHealth;
        _enemySprite.sprite = enemyData.sprite;
        _enemySprite.color = Color.white;

        increaseReceiveDamageRate = 0f;
    }

    public virtual void TakeDamage(float damage, DamageTakenType damageTakenType)
    {
        StartCoroutine(EnemyTakeDamageAnimationCoroutine());

        damage *= 1f + (increaseReceiveDamageRate * 0.01f);
        _health -= damage;
        _enemyHealthbar.health -= damage;

        UIManager.instance.ShowDamageTakenText(damage, this.transform, damageTakenType);

        if (_health <= 0)
            Die();
    }

    public void TakeIncreaseReceivedDamage(float increaseReceivedDamageRate, float duration)
    {
        StartCoroutine(IncreaseReceivedDamageCoroutine(increaseReceivedDamageRate, duration));
    }
    private IEnumerator IncreaseReceivedDamageCoroutine(float IRDRate, float duration)
    {
        Debug.Log("������ �� ���ط�: " + increaseReceiveDamageRate);
        this.increaseReceiveDamageRate += IRDRate;

        Debug.Log("�޴����ط� ����: " + IRDRate);
        Debug.Log("������ �� ���ط�: " + increaseReceiveDamageRate);

        // duration��ŭ ����
        while (duration > 0)
        {
            //yield return null;
            yield return _waitForPointFiveSeconds;
            duration -= 0.5f;
        }

        Debug.Log("������ �� ���ط�: " + increaseReceiveDamageRate);
        this.increaseReceiveDamageRate -= IRDRate;

        Debug.Log("�޴����ط� ����: " + IRDRate);
        Debug.Log("���ҵ� �� ���ط�: " + increaseReceiveDamageRate);
    }

    public abstract void TakeStun(float duration);
    public abstract void TakeSlowing(float slowingRate, float duration);

    protected virtual void Die()
    {
        ParticlePlayer.instance.PlayEnemyDie(this.transform);
        GiveReward();
    }

    private void GiveReward()
    {
        GameManager.instance.gold += _rewardGold;
        if (_rewardChangeChance > 0)
            GameManager.instance.changeChance += _rewardChangeChance;

        UIManager.instance.ShowEnemyDieRewardText(_rewardText, this.transform);
    }

    private void SetRewardText()
    {
        _rewardText.Append('+');
        _rewardText.Append(_rewardGold.ToString());
        _rewardText.Append('G');

        if(_rewardChangeChance > 0)
        {
            _rewardText.Append('\n');
            _rewardText.Append("ī�屳ȯ��");
            _rewardText.Append('+');
            _rewardText.Append(_rewardChangeChance.ToString());
        }
    }

    private IEnumerator EnemyTakeDamageAnimationCoroutine()
    {
        _enemySprite.color = Color.red;

        yield return _takeDamageAnimationDelay;

        _enemySprite.color = Color.white;
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
