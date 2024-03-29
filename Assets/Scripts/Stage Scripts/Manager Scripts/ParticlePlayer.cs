using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private static ParticlePlayer s_instance;
    public static ParticlePlayer instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<ParticlePlayer>();
                return s_instance;
            }

            return s_instance;
        }
    }

    [SerializeField]
    private GameObject _enemyDiePrefab;
    [SerializeField]
    private GameObject[] _rangeAttackPrefabs;
    [SerializeField]
    private GameObject[] _collisionProjectilePrefabs;

    private readonly WaitForSeconds _delayTime = new(0.5f);
    private readonly WaitForSeconds _collisionProjectileDelayTime = new(0.15f);

    private ObjectPool<Particle> _enemyDiePool;
    private ObjectPool<Particle>[] _rangeAttackPools = new ObjectPool<Particle>[3];
    private ObjectPool<Particle>[] _collisionProjectilePools = new ObjectPool<Particle>[3];

    private void Awake()
    {
        _enemyDiePool = new(_enemyDiePrefab, 10);

        for (int i = 0; i < _rangeAttackPrefabs.Length; i++)
            _rangeAttackPools[i] = new ObjectPool<Particle>(_rangeAttackPrefabs[i], 5);


        for (int i = 0; i < _rangeAttackPrefabs.Length; i++)
            _collisionProjectilePools[i] = new ObjectPool<Particle>(_collisionProjectilePrefabs[i], 10);
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
        Particle particle = _collisionProjectilePools[index].GetObject();
        particle.transform.position = projectileTransform.position;
        particle.transform.localScale = projectileTransform.lossyScale;
        particle.PlayParticle();

        StartCoroutine(CollisionProjectileReturnPoolCoroutine(particle, index));
    }
    private IEnumerator CollisionProjectileReturnPoolCoroutine(Particle particle, int index)
    {
        yield return _collisionProjectileDelayTime;
        _collisionProjectilePools[index].ReturnObject(particle);
    }


    public void PlayRangeAttack(Transform targetTransform, float range, int index)
    {
        Particle particle = _rangeAttackPools[index].GetObject();
        particle.transform.position = targetTransform.position;
        particle.transform.localScale = new Vector3(range * 2, range * 2, 0f);
        particle.PlayParticle();

        StartCoroutine(RangeAttackReturnPoolCoroutine(particle, index));
    }
    private IEnumerator RangeAttackReturnPoolCoroutine(Particle particle, int index)
    {
        yield return _delayTime;
        _rangeAttackPools[index].ReturnObject(particle);
    }

}

/*
 * File : ParticlePlayer.cs
 * 
 * 파티클을 실행하자마자 비활성화 되는 오브젝트들의 파티클을 플레이 해주는 스크립트.
 */