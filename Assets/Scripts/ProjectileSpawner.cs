using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;
    private ObjectPool<Projectile> _projectilePool;
    private List<Projectile> _projectileList;

    public ObjectPool<Projectile> projectilePool => _projectilePool;
    public List<Projectile> projectileList => _projectileList;


    private void Awake()
    {
        _projectilePool = new ObjectPool<Projectile>(_projectilePrefab, 30);
        _projectileList = new();
    }

    public Projectile SpawnProjectile(Tower fromTower, Transform spawnPoint, Enemy target, Sprite projectileSprite)
    {
        Projectile _projectile;

        _projectile = _projectilePool.GetObject();
        _projectile.transform.position = spawnPoint.position;
        _projectile.Setup(fromTower, target, projectileSprite);
        _projectileList.Add(_projectile);
        return _projectile;
    }
}
