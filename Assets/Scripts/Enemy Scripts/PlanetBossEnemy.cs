using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBossEnemy : Enemy
{
    [SerializeField]
    private Rotater2D _rotater2D;
    [SerializeField]
    private float _planetHealth;

    protected override void Awake()
    {
        base.Awake();
        _rotater2D = GetComponent<Rotater2D>();

        // 생성할 Planet의 체력 설정
        _maxHealth = _planetHealth;
        _health = _maxHealth;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _maxHealth;
    }

    private void Update()
    {
        _rotater2D.NaturalRotate();
    }

    public override void OnDamage(float damage)
    {
        // Planet Boss는 공격력에 상관없이 1의 데미지를 받음.
        damage = 1;
        base.OnDamage(damage);
    }

    protected override void Die()
    {
        // 플레이어에게 카드 변경권 5장을 지급한다.
        GameManager.instance.IncreaseChangeChance(5);

        ResetPlanet();
    }

    private void ResetPlanet()
    {
        _health = _maxHealth;
        _healthSlider.value = _maxHealth;
    }

}
