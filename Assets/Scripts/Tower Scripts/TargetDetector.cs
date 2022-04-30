using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    private TowerWeapon _towerWeapon;
    private Transform _target;

    public Transform target => _target;

    public void DefaultSetup(EnemySpawner enemySpawner)
    {
        if (_enemySpawner == null)
        {
            _enemySpawner = enemySpawner;
        }
        _towerWeapon = GetComponentInChildren<TowerWeapon>();
        _target = null;
    }

    public void StartSearchTarget() => StartCoroutine(SearchTarget());
    public void StopSearchTarget() => StopCoroutine(SearchTarget());

    private IEnumerator SearchTarget()
    {
        float closestDistSqr, distance;

        while (true)
        {
            if (target == null)
            {
                closestDistSqr = Mathf.Infinity;

                for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
                {
                    distance = Vector3.Distance(this.transform.position, _enemySpawner.enemyList[i].transform.position);

                    if (distance <= _towerWeapon.range && distance <= closestDistSqr)
                    {
                        closestDistSqr = distance;
                        _target = _enemySpawner.enemyList[i].transform;
                    }
                }
            }
            else
            {
                distance = Vector3.Distance(this.transform.position, _target.transform.position);
                if (distance > _towerWeapon.range)
                    _target = null;
            }

            yield return null;
        }
    }
}

/*
 * File : TargetDetector.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower의 타겟 탐지 기능을 담당하는 스크립트.
 * EnemySpawner 컴포넌트를 통해 살아있는 Enemy 오브젝트들의 정보를 가져오고, 
 * TowerWeapon 컴포넌트를 통해 얻은 타워의 공격 범위 내에 있는 Enemy 오브젝트를 찾도록 구현.
 */