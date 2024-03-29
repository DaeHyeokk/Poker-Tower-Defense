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

    public event Action actionOnCollision;

    private void Awake()
    {
        _projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement2D = GetComponent<Movement2D>();
        _rotater2D = GetComponent<Rotater2D>();
    }

    public void Setup(Tower fromTower, Enemy target, Sprite projectileSprite)
    {
        _fromTower = fromTower;
        _target = target;
        _spriteRenderer.sprite = projectileSprite;
        _rotater2D.LookAtTarget(_target.transform);
    }

    private void Update()
    {
        // _target 오브젝트가 씬에 활성화된 상태라면 _target을 계속 추적한다.
        if (_target.gameObject.activeSelf)
        {
            // target과의 거리가 0.37f 이하라면 충돌했다고 판정한다.
            if (Vector2.Distance(this.transform.position, _target.transform.position) <= 0.37f)
            {
                ParticlePlayer.instance.PlayCollisionProjectile(this.transform, (int)_fromTower.towerColor.colorType);

                if (actionOnCollision != null)
                    actionOnCollision();

                ReturnPool();
            }
            else
            {
                Vector3 direction = (_target.transform.position - this.transform.position).normalized;
                _movement2D.MoveTo(direction);
                _rotater2D.LookAtTarget(_target.transform);
            }
        }
        else
            ReturnPool();
    }

    private void ReturnPool()
    {
        actionOnCollision = null;
        _movement2D.MoveTo(Vector3.zero);
        _projectileSpawner.projectilePool.ReturnObject(this);
    }
}


/*
 * File : Projectile.cs
 * First Update : 2022/04/28 THU 23:55
 * 타워의 총알 발사를 확인하기 위해 간단히 구현한 스크립트.
 * 추후에 Projectile 클래스를 추상클래스로 선언하여 다양한 Projectile을 구현할 예정.
 */