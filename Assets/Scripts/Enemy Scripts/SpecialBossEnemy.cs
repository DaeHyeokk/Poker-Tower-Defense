using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialBossEnemy : Enemy
{
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private float _specialBossHealth;

    protected void Awake()
    {
        // 생성할 Planet의 체력 설정
        _maxHealth = _specialBossHealth;
        _health = _maxHealth;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = 0;
    }

    public override void OnDamage(float damage)
    {
        // Special Boss는 공격력에 상관없이 1의 데미지를 받음.
        _health--;
        _healthSlider.value++;
        _healthText.text = _health.ToString();

        if (_health <= 0)
            Die();
    }

    // Special Bass는 스턴과 슬로우 공격을 받지 않음 //
    public override void OnStun(float stunTime)
    {
        return;
    }

    public override void OnSlow(float slowPer, float slowTime)
    {
        return;
    }
    //////////////////////////////////////////////////////////////
   
    protected override void Die()
    {
        // 플레이어에게 카드 변경권 5장을 지급한다.
        GameManager.instance.IncreaseChangeChance(5);

        ResetPlanet();
    }

    private void ResetPlanet()
    {
        _health = _maxHealth;
        _healthText.text = _maxHealth.ToString();
        _healthSlider.value = 0;
    }

}
