using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    private ProjectileSpawner _projectileSpawner;
    private SpriteRenderer _spriteRenderer;
    private Movement2D _movement2D;

    private Transform _target;
    private Tower _fromTower;

    public event Action actionOnCollision;

    private void Awake()
    {
        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement2D = GetComponent<Movement2D>();
    }

    public void Setup(Transform targetTransfrom, Sprite projectileSprite)
    {
        _target = targetTransfrom;
        _spriteRenderer.sprite = projectileSprite;
    }

    private void Update()
    {
        if(_target != null)
        {
            Vector3 direction = (_target.position - this.transform.position).normalized;
            _movement2D.MoveTo(direction);
        }
        else
        {
            ReturnPool();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != _target) return;

        actionOnCollision();
        ReturnPool();
    }

    public void ReturnPool()
    {
        actionOnCollision = null;
        _projectileSpawner.projectilePool.ReturnObject(this);
    }
}


/*
 * File : Projectile.cs
 * First Update : 2022/04/28 THU 23:55
 * Ÿ���� �Ѿ� �߻縦 Ȯ���ϱ� ���� ������ ������ ��ũ��Ʈ.
 * ���Ŀ� Projectile Ŭ������ �߻�Ŭ������ �����Ͽ� �پ��� Projectile�� ������ ����.
 */