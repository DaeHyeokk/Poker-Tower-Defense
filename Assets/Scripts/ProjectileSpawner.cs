using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;

    private ObjectPool<Projectile> _projectilePool;

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(_projectilePrefab, 50);
    }

    public void SpawnProjectile(Vector3 spawnPoint, Transform target)
    {
        Projectile projectile = _projectilePool.GetObject();
        projectile.transform.position = spawnPoint;
        projectile.Setup(target);
    }
}


/*
 * File : ProjectileSpawner.cs
 * First Update : 2022/04/25 MON 10:52
 * Ÿ���� ������ �� �����Ǵ� �߻�ü�� ������ ����ϴ� ��ũ��Ʈ.
 */