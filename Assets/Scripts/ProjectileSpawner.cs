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

    public Projectile SpawnProjectile(Tower fromTower, Transform spawnPoint, Enemy target, Sprite projectileSprite)
    {
        _projectile = _projectilePool.GetObject();
        _projectile.transform.position = spawnPoint.position;
        _projectile.Setup(fromTower, target, projectileSprite);
        return _projectile;
    }
}
