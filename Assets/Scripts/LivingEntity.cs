using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingEntity : MonoBehaviour
{
    private float _maxHealth;
    private float _health;

    public event Action actionOnDeath;

    public float maxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float health
    {
        get => _health;
        set => _health = value;
    }

    private void OnEnable()
    {
        actionOnDeath = null;
    }

    public void OnDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if(actionOnDeath != null)
            actionOnDeath();
    }
}
