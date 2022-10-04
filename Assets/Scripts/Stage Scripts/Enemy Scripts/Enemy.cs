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

    // Enemy를 잡을 경우 플레이어에게 지급되는 골드
    protected int _rewardGold;
    // Enemy를 잡을 경우 플레이어에게 지급되는 카드교환권
    protected int _rewardChangeChance;
    protected RewardStringBuilder _rewardStringBuilder;

    protected float _maxHealth;  // Enemy의 최대 체력
    protected float _health;     // Enemy의 현재 체력
    private float _increaseReceiveDamageRate; // Enemy가 공격 당할 때 받는 피해량
    private IEnumerator _increaseReceivedDamageCoroutine;
    protected EnemySpawner _enemySpawner;
    protected EnemyHealthbar _enemyHealthbar;

    private readonly WaitForSeconds _takeDamageAnimationDelay = new(0.05f);

    protected float maxHealth => _maxHealth;  // Enemy의 최대 체력
    protected float health => _health;  // Enemy의 현재 체력
    private float increaseReceiveDamageRate
    {
        get => _increaseReceiveDamageRate;
        set
        {
            // increaseReceiveDamageRate 값이 0이고 value값이 0보다 크다면 디버프 스프라이트를 활성화 한다.
            if (_increaseReceiveDamageRate == 0 && value > 0)
                _increaseReceiveDamageSprite.gameObject.SetActive(true);
            // increaseReceiveDamageRate 값이 0이 아니고 value값이 0이라면 디버프 스프라이트를 비활성화 한다.
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
        // 생성할 Enemy의 스프라이트 설정 및 색깔 초기화
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
        // 몬스터가 이미 받는 피해량 증가 디버프를 받고 있을 경우 수행.
        if (this.increaseReceiveDamageRate != 0f)
        {
            // 더 큰 값의 받는 피해량 증가 디버프를 적용받고 있을 경우 건너뛴다. 
            if (this.increaseReceiveDamageRate > increaseReceiveDamageRate)
                return;
            // 더 큰 값의 받는 피해량 증가 디버프를 적용하는 경우, 이전에 받고 있던 디버프를 제거한다.
            else
                StopCoroutine(_increaseReceivedDamageCoroutine);
        }

        // 디버프 적용.
        _increaseReceivedDamageCoroutine = IncreaseReceivedDamageCoroutine(increaseReceiveDamageRate, duration);
        StartCoroutine(_increaseReceivedDamageCoroutine);
    }

    private IEnumerator IncreaseReceivedDamageCoroutine(float IRDRate, float duration)
    {
        this.increaseReceiveDamageRate = IRDRate;

        // duration만큼 지연
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
        // 게임이 종료된 상태가 아닐 경우, FromTower의 킬수 카운트 증가.
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
