using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector
{
    public enum DetectingMode { Single, Multiple }

    private Tower _tower;
    private EnemySpawner _enemySpawner;
    private List<Enemy> _targetList;

    private Enemy _tempTarget;
    private float _distance;
    private DetectingMode _detectingMode;

    public DetectingMode detectingMode { set => _detectingMode = value; }
    public List<Enemy> targetList => _targetList;

    public TargetDetector(Tower tower, EnemySpawner enemySpawner)
    {
        _tower = tower;
        _enemySpawner = enemySpawner;
        _targetList = new List<Enemy>(_tower.maxTargetCount);
        _distance = 0;
    }

    public void SearchTarget()
    {
        // ���� Ÿ�� Ÿ���� ��� ����
        if (_detectingMode == DetectingMode.Single)
        {
            // ��Ÿ� ���� ����Ⱥ����� �ִٸ� ���� �켱 Ÿ���Ѵ�.
            _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.specialBossEnemy.transform.position);
            if (_distance <= _tower.range)
            {
                if (_targetList.Count != 0)
                    _targetList.RemoveAt(0);

                _targetList.Add(_enemySpawner.specialBossEnemy);
                return;
            }

            // Ÿ���� ������ �켱 Ÿ���ϱ� ������ ���� ���͸� ���� Ž���Ѵ�.
            for (int i = 0; i < _enemySpawner.missionBossEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.missionBossEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                {
                    if (_targetList.Count != 0)
                        _targetList.RemoveAt(0);

                    _targetList.Add(_enemySpawner.missionBossEnemyList[i]);
                    return;
                }
            }

            if (_targetList.Count != 0)
            {
                _distance = Vector3.Distance(_tower.transform.position, _targetList[0].transform.position);
                if (_distance > _tower.range || !_targetList[0].gameObject.activeInHierarchy)
                    _targetList.RemoveAt(0);
                else
                    return;
            }

            float _closestDistSqr = Mathf.Infinity;

            for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.roundEnemyList[i].transform.position);
                if (_distance <= _tower.range && _distance <= _closestDistSqr)
                {
                    _closestDistSqr = _distance;
                    _tempTarget = _enemySpawner.roundEnemyList[i];
                }
            }

            if (_tempTarget != null)
            {
                targetList.Add(_tempTarget);
                _tempTarget = null;
            }
        }
        // ���� Ÿ�� Ÿ���� ��� ����
        else // (_detectingMode == DetectingMode.Multiple)
        {
            _targetList.Clear();

            // ��Ÿ� ���� ����Ⱥ����� �ִٸ� ���� �켱 Ÿ���Ѵ�.
            _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.specialBossEnemy.transform.position);
            if (_distance <= _tower.range)
            {
                if (_targetList.Count != 0)
                    _targetList.RemoveAt(0);

                _targetList.Add(_enemySpawner.specialBossEnemy);
            }

            // Ÿ���� ������ �켱 Ÿ���ϱ� ������ ���� ���͸� ���� Ž���Ѵ�.
            for (int i = 0; i < _enemySpawner.missionBossEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.missionBossEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                    _targetList.Add(_enemySpawner.missionBossEnemyList[i]);

                if (_targetList.Count >= _tower.maxTargetCount)
                    break;
            }

            for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.roundEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                {
                    _targetList.Add(_enemySpawner.roundEnemyList[i]);
                }

                if (_targetList.Count >= _tower.maxTargetCount)
                    break;
            }
        }
    }

    public void ResetTarget()
    {
        _targetList.Clear();
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
 * 
 * Update : 2022/05/03 THU 03:13
 * Ÿ���� ��Ÿ��� �÷��̾�� �����ִ� ���� �߰�.
 */