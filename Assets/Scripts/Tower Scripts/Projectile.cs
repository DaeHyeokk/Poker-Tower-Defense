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
        // _target 오브젝트가 씬에 활성화된 상태라면 _target을 계속 추적한다.
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
 * 타워의 총알 발사를 확인하기 위해 간단히 구현한 스크립트.
 * 추후에 Projectile 클래스를 추상클래스로 선언하여 다양한 Projectile을 구현할 예정.
 */