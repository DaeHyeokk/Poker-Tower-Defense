using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private static ParticlePlayer _instance;
    public static ParticlePlayer instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticlePlayer>();
                return _instance;
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _enemyDiePrefab;
    [SerializeField]
    private GameObject[] _rangeAttackPrefabs;
    [SerializeField]
    private GameObject[] _collisionProjectilePrefabs;

    private WaitForSeconds _delayTime;
    private WaitForSeconds _collisionProjectileDelayTime;
    private ObjectPool<Particle> _enemyDiePool;
    private List<ObjectPool<Particle>> _rangeAttackPoolList;
    private List<ObjectPool<Particle>> _collisionProjectilePoolList;

    public ObjectPool<Particle> enemyDiePool => _enemyDiePool;

    private void Awake()
    {
        _delayTime = new WaitForSeconds(0.5f);
        _collisionProjectileDelayTime = new WaitForSeconds(0.15f);
        _enemyDiePool = new(_enemyDiePrefab, 10);

        _rangeAttackPoolList = new List<ObjectPool<Particle>>();

        for (int i = 0; i < _rangeAttackPrefabs.Length; i++)
            _rangeAttackPoolList.Add(new(_rangeAttackPrefabs[i], 5));

        _collisionProjectilePoolList = new List<ObjectPool<Particle>>();

        for (int i = 0; i < _rangeAttackPrefabs.Length; i++)
            _collisionProjectilePoolList.Add(new(_collisionProjectilePrefabs[i], 10));
    }

    public void PlayEnemyDie(Transform enemyTransform)
    {
        Particle particle = _enemyDiePool.GetObject();
        particle.transform.position = enemyTransform.position;
        particle.transform.localScale = enemyTransform.lossyScale;
        particle.PlayParticle();

        StartCoroutine(EnemyDieReturnPoolCoroutine(particle));
    }
    private IEnumerator EnemyDieReturnPoolCoroutine(Particle particle)
    {
        yield return _delayTime;
        _enemyDiePool.ReturnObject(particle);
    }


    public void PlayCollisionProjectile(Transform projectileTransform, int index)
    {
        Particle particle = _collisionProjectilePoolList[index].GetObject();
        particle.transform.position = projectileTransform.position;
        particle.transform.localScale = projectileTransform.lossyScale;
        particle.PlayParticle();

        StartCoroutine(CollisionProjectileReturnPoolCoroutine(particle, index));
    }
    private IEnumerator CollisionProjectileReturnPoolCoroutine(Particle particle, int index)
    {
        yield return _collisionProjectileDelayTime;
        _collisionProjectilePoolList[index].ReturnObject(particle);
    }


    public void PlayRangeAttack(Transform targetTransform, float range, int index)
    {
        Particle particle = _rangeAttackPoolList[index].GetObject();
        particle.transform.position = targetTransform.position;
        particle.transform.localScale = new Vector3(range * 2, range * 2, 0f);
        particle.PlayParticle();

        StartCoroutine(RangeAttackReturnPoolCoroutine(particle, index));
    }
    private IEnumerator RangeAttackReturnPoolCoroutine(Particle particle, int index)
    {
        yield return _delayTime;
        _rangeAttackPoolList[index].ReturnObject(particle);
    }

}
