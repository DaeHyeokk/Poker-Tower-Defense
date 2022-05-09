using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    private ProjectileSpawner _projectileSpawner;
    private SpriteRenderer _spriteRenderer;
    private Movement2D _movement2D;
    private Rotater2D _rotater2D;
    private Enemy _target;
    private Tower _fromTower;

    private bool _isCollision;
    public event Action actionOnCollision;

    private void Awake()
    {
        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement2D = GetComponent<Movement2D>();
        _rotater2D = GetComponent<Rotater2D>();

        _movement2D.moveSpeed = 10f;
    }

    public void Setup(Tower fromTower, Enemy target, Sprite projectileSprite)
    {
        _fromTower = fromTower;
        _target = target;
        _spriteRenderer.sprite = projectileSprite;
        _isCollision = false;
    }

    private void Update()
    {
        // _target ������Ʈ�� ���� Ȱ��ȭ�� ���¶�� _target�� ��� �����Ѵ�.
        if (_target.gameObject.activeInHierarchy)
        {
            Vector3 direction = (_target.transform.position - this.transform.position).normalized;
            _movement2D.MoveTo(direction);
            _rotater2D.ProjectileRotate(_fromTower.towerRenderer.transform.rotation);
        }
        else
        {
            ReturnPool();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") 
            || !_target.gameObject.activeInHierarchy 
            || collision.transform != _target.transform) return;

        _isCollision = true;

        if(actionOnCollision != null)
            actionOnCollision();
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