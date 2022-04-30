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
 * Tower�� Ÿ�� Ž�� ����� ����ϴ� ��ũ��Ʈ.
 * EnemySpawner ������Ʈ�� ���� ����ִ� Enemy ������Ʈ���� ������ ��������, 
 * TowerWeapon ������Ʈ�� ���� ���� Ÿ���� ���� ���� ���� �ִ� Enemy ������Ʈ�� ã���� ����.
 */