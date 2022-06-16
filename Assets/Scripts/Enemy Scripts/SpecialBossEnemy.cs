using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialBossEnemy : Enemy
{
    [SerializeField]
    private Rotater2D _rotater2D;
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private float _specialBossHealth;

    protected override void Awake()
    {
        base.Awake();

        // ������ Planet�� ü�� ����
        maxHealth = _specialBossHealth;
        health = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = 0;
    }

    private void Update()
    {
        _rotater2D.NaturalRotate();
    }

    public override void TakeDamage(float damage, DamageTakenType damageTakenType)
    {
        base.TakeDamage(damage, damageTakenType);

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
        base.Die();
        this.gameObject.SetActive(false);
        // �÷��̾�� ī�� ����� 5���� �����Ѵ�.
        GameManager.instance.changeChance += 5;

        Invoke("RespawnPlanet", 0.4f);
    }

    private void RespawnPlanet()
    {
        health = maxHealth;
        _healthText.text = maxHealth.ToString();
        healthSlider.value = 0;

        enemySprite.color = Color.white;
        this.gameObject.SetActive(true);
    }
}
