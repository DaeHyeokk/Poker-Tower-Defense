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

        // ������ Planet�� ü�� ����
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
        // Planet Boss�� ���ݷ¿� ������� 1�� �������� ����.
        damage = 1;
        base.OnDamage(damage);
    }

    protected override void Die()
    {
        // �÷��̾�� ī�� ����� 5���� �����Ѵ�.
        GameManager.instance.IncreaseChangeChance(5);

        ResetPlanet();
    }

    private void ResetPlanet()
    {
        _health = _maxHealth;
        _healthSlider.value = _maxHealth;
    }

}
