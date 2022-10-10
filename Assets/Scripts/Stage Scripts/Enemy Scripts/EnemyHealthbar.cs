using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbar
{
    private SpriteRenderer _healthbarGauge;
    private float _maxHealth;  // Enemy�� �ִ� ü��
    private float _health;     // Enemy�� ���� ü��
    private float _healthRatio;

    public float maxHealth
    {
        get => _maxHealth;
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
            UpdateHealthbarColor();
        }
    }

    public float healthPercent => _healthRatio * 100f;
     
    public EnemyHealthbar(SpriteRenderer healthbarGauge)
    {
        _healthbarGauge = healthbarGauge;
    }

    private void UpdateHealthbarGauge()
    {
        _healthRatio = _health / _maxHealth;
        float gaugeXpos = -((1-_healthRatio) * 0.5f);

        _healthbarGauge.transform.localScale = new Vector3(_healthRatio, 1f, 1f);
        _healthbarGauge.transform.localPosition = new Vector3(gaugeXpos, 0f, 0f);
    }

    private void UpdateHealthbarColor()
    {
        switch (_healthRatio)
        {
            case > 0.8f:
                _healthbarGauge.color = Color.green;
                break;

            case > 0.6f:
                _healthbarGauge.color = new Color(1f, 0.8f, 0f); // Dark Yellow
                break;

            case > 0.4f:
                _healthbarGauge.color = new Color(1f, 0.5f, 0f); // Orange
                break;

            default:
                _healthbarGauge.color = Color.red;
                break;
        }
    }
}


/*
 * File : EnemyHelthbar.cs
 * First Update : 2022/06/28 THU 04:30
 * Canvas.BuildBatch() ���� ���� ������ �ذ��ϰ��� Slider�� ����� ������ ��ũ��Ʈ.
 * healthGauge�� localScale X���� localPosition�� ������ ������ �Բ� �ٲ������ν� Slideró�� �����ϵ��� �����Ͽ���.
 * 
 * Update : 2022/06/29 WED 05:46
 * ���� ü�� �ۼ�Ʈ ���� ���� �÷��� �����Ű�� ���� �߰�.
 */