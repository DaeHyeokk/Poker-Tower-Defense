using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    private TowerAttacker _towerAttacker;
    private Transform _target;

    public Transform target => _target;

    private void OnDisable()
    {
        _target = null;
    }

    public void Setup(EnemySpawner enemySpawner)
    {
        if (_enemySpawner == null)
        {
            _enemySpawner = enemySpawner;
            _towerAttacker = GetComponent<TowerAttacker>();
        }
    }

    public void StartSearchTarget() => StartCoroutine(SearchTarget());

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closestDistSqr = Mathf.Infinity;

            for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
            {
                float distance = Vector3.Distance(this.transform.position, _enemySpawner.enemyList[i].transform.position);

                if (distance <= _towerAttacker.towerWeapon.attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    _target = _enemySpawner.enemyList[i].transform;
                }
            }

            if (_target != null)
            {
                _towerAttacker.StartAttackToTarget();
                StopCoroutine(SearchTarget());
            }

            yield return null;
        }
    }
}
