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

       //_movement2D.moveSpeed = 6f;
    }

    public void Setup(Tower fromTower, Enemy target, Sprite projectileSprite)
    {
        _fromTower = fromTower;
        _target = target;
        _spriteRenderer.sprite = projectileSprite;
        _rotater2D.LookAtTarget(_target.transform);
    }

    private void FixedUpdate()
    {
        // _target ������Ʈ�� ���� Ȱ��ȭ�� ���¶�� _target�� ��� �����Ѵ�.
        if (_target.gameObject.activeSelf)
        {
            // target���� �Ÿ��� 0.4f ���϶�� �浹�ߴٰ� �����Ѵ�.
            if (Vector2.Distance(this.transform.position, _target.transform.position) <= 0.4f)
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
 * Ÿ���� �Ѿ� �߻縦 Ȯ���ϱ� ���� ������ ������ ��ũ��Ʈ.
 * ���Ŀ� Projectile Ŭ������ �߻�Ŭ������ �����Ͽ� �پ��� Projectile�� ������ ����.
 */