using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    private Slider _healthSlider;
    private SpriteRenderer _enemySprite;
    private WaitForSeconds _takeDamageAnimationDelay;

    protected Slider healthSlider => _healthSlider;
    protected SpriteRenderer enemySprite => _enemySprite;
    protected float maxHealth { get; set; }  // Enemy의 최대 체력
    protected float health { get; set; }     // Enemy의 현재 체력

    protected virtual void Awake()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        _enemySprite = GetComponentInChildren<SpriteRenderer>();
        _takeDamageAnimationDelay = new WaitForSeconds(0.05f);
    }

    public virtual void TakeDamage(float damage)
    {
        StartCoroutine(EnemyTakeDamageAnimationCoroutine());
    }

    public abstract void TakeIncreaseReceivedDamage(float increaseReceivedDamageRate, float duration);
    public abstract void TakeStun(float duration);
    public abstract void TakeSlowing(float slowingRate, float duration);
    protected virtual void Die()
    {
        ParticlePlayer.instance.PlayEnemyDie(this.transform);
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
