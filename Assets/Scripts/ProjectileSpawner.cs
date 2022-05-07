using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;
    private Projectile _projectile;
    private ObjectPool<Projectile> _projectilePool;

    public ObjectPool<Projectile> projectilePool => _projectilePool;

    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(_projectilePrefab, 30);
    }

    public Projectile SpawnProjectile(Transform spawnPoint, Transform target, Sprite projectileSprite)
    {
        _projectile = _projectilePool.GetObject();
        _projectile.transform.position = spawnPoint.position;
        _projectile.Setup(target, projectileSprite);

        return _projectile;
    }
}
