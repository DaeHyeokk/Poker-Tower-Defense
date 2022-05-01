using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    private TowerWeapon _towerWeapon;
    private Enemy _enemyTarget;

    public Enemy enemyTarget => _enemyTarget;

    public void DefaultSetup(EnemySpawner enemySpawner)
    {
        if (_enemySpawner == null)
        {
            _enemySpawner = enemySpawner;
        }
        _towerWeapon = GetComponentInChildren<TowerWeapon>();
        _enemyTarget = null;
    }

    public void StartSearchTarget() => StartCoroutine(SearchTarget());
    public void StopSearchTarget() => StopCoroutine(SearchTarget());

    private IEnumerator SearchTarget()
    {
        float closestDistSqr, distance;

        while (true)
        {
            if (_enemyTarget == null)
            {
                closestDistSqr = Mathf.Infinity;

                for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
                {
                    distance = Vector3.Distance(this.transform.position, _enemySpawner.enemyList[i].transform.position);

                    if (distance <= _towerWeapon.range && distance <= closestDistSqr)
                    {
                        closestDistSqr = distance;
                        _enemyTarget = _enemySpawner.enemyList[i];
                    }
                }

                if (_enemyTarget != null)
                    _enemyTarget.actionOnDisable += EnemyTargetReset;
            }
            else
            {
                distance = Vector3.Distance(this.transform.position, _enemyTarget.transform.position);
                if (distance > _towerWeapon.range)
                {
                    _enemyTarget.actionOnDisable -= EnemyTargetReset;
                    _enemyTarget = null;
                }
            }

            yield return null;
        }
    }

    private void EnemyTargetReset()
    {
        _enemyTarget = null;
    }
}

/*
 * File : TargetDetector.cs
 * First Update : 2022/04/30 SAT 23:50
 * Tower�� Ÿ�� Ž�� ����� ����ϴ� ��ũ��Ʈ.
 * EnemySpawner ������Ʈ�� ���� ����ִ� Enemy ������Ʈ���� ������ ��������, 
 * TowerWeapon ������Ʈ�� ���� ���� Ÿ���� ���� ���� ���� �ִ� Enemy ������Ʈ�� ã���� ����.
 * 
 * Update : 2022/05/01 SUM 15:30
 * Ÿ���� ��Ÿ��� ����� ��Ȳ �ܿ� Ÿ���� �װų�, ���� ������ ���� ��Ȳ�� ���� Target�� Reset�ϴ� ���� �߰�.
 */