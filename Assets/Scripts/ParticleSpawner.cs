using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    private static ParticleSpawner _instance;
    public static ParticleSpawner instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticleSpawner>();
                return _instance;
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _enemyDiePrefab;

    private ObjectPool<ParticleSystem> _enemyDiePool;

    public ObjectPool<ParticleSystem> enemyDiePool => _enemyDiePool;

    private void Awake()
    {
        _enemyDiePool = new(_enemyDiePrefab, 10);
    }

    public void PlayParticle(Vector3 startPosition, Vector3 startScale)
    {
        ParticleSystem particle = _enemyDiePool.GetObject();
        particle.transform.position = startPosition;
        particle.transform.localScale = startScale;
        particle.Play();

        StartCoroutine(ReturnPoolCoroutine(particle));
    }

    private IEnumerator ReturnPoolCoroutine(ParticleSystem particle)
    {
        yield return new WaitForSeconds(0.4f);
        _enemyDiePool.ReturnObject(particle);
    }
}
