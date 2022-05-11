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
        // ������ Planet�� ü�� ����
        _maxHealth = _specialBossHealth;
        _health = _maxHealth;

        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = 0;
    }

    public override void OnDamage(float damage)
    {
        // Special Boss�� ���ݷ¿� ������� 1�� �������� ����.
        _health--;
        _healthSlider.value++;
        _healthText.text = _health.ToString();

        if (_health <= 0)
            Die();
    }

    // Special Bass�� ���ϰ� ���ο� ������ ���� ���� //
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
        // �÷��̾�� ī�� ����� 5���� �����Ѵ�.
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
