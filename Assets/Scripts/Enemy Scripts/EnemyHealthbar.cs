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
 * Canvas.BuildBatch() 성능 저하 문제를 해결하고자 Slider의 기능을 구현한 스크립트.
 * healthGauge의 localScale X값과 localPosition을 일정한 비율로 함께 바꿔줌으로써 Slider처럼 동작하도록 구현하였음.
 * */