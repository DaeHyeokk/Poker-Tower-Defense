using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbar
{
    private Transform _healthbarGauge;   
    private float _maxHealth;
    private float _health;

    public float maxHealth
    {
        set => _maxHealth = value;
    }

    public float health
    {
        get => _health;
        set
        {
            if (value < 0f) value = 0f;
            _health = value;
            UpdateHealthbarGauge();
        }
    }

    public EnemyHealthbar(Transform healthbarGauge)
    {
        _healthbarGauge = healthbarGauge;
    }

    private void UpdateHealthbarGauge()
    {
        float healthRatio = _health / _maxHealth;
        float gaugeXpos = -((1-healthRatio) * 0.5f);

        _healthbarGauge.localScale = new Vector3(healthRatio, 1f, 1f);
        _healthbarGauge.localPosition = new Vector3(gaugeXpos, 0f, 0f);
    }
}


/*
 * File : EnemyHelthbar.cs
 * First Update : 2022/06/28 THU 04:30
 * Canvas.BuildBatch() ���� ���� ������ �ذ��ϰ��� Slider�� ����� ������ ��ũ��Ʈ.
 * healthGauge�� localScale X���� localPosition�� ������ ������ �Բ� �ٲ������ν� Slideró�� �����ϵ��� �����Ͽ���.
 * */