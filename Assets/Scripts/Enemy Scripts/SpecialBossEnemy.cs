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
        maxHealth = _specialBossHealth;
        health = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = 0;
    }

    public override void TakeDamage(float damage)
    {
        // Special Boss�� ���ݷ¿� ������� 1�� �������� ����.
        health--;
        healthSlider.value++;
        _healthText.text = health.ToString();

        if (health <= 0)
            Die();
    }

    // Special Bass�� ���ϰ� ���ο�, �޴� ���ط� ���� ������� ���� ���� //
    public override void TakeStun(float duration)
    {
        return;
    }

    public override void TakeSlowing(float slowingRate, float duration)
    {
        return;
    }

    public override void TakeIncreaseReceivedDamage(float receivedDamageRate, float duration)
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
        health = maxHealth;
        _healthText.text = maxHealth.ToString();
        healthSlider.value = 0;
    }

}
