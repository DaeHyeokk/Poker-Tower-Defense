using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector
{
    public enum DetectingMode { Single, Multiple }

    private Tower _tower;
    private List<Enemy> _targetList;
    private Enemy _tempTarget;
    private float _distance;
    private DetectingMode _detectingMode;

    public DetectingMode detectingMode { set => _detectingMode = value; }
    public List<Enemy> targetList => _targetList;

    public TargetDetector(Tower tower)
    {
        _tower = tower;
        _targetList = new List<Enemy>(_tower.maxTargetCount);
        _distance = 0;
    }

    public void SearchTarget()
    {
        // ���� Ÿ�� Ÿ���� ��� ����
        if (_detectingMode == DetectingMode.Single)
        {
            // ��Ÿ� ���� ����Ⱥ����� �ִٸ� ���� �켱 Ÿ���Ѵ�.
            _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.specialBossEnemy.transform.position);
            if (_distance <= _tower.range)
            {
                _targetList.Clear();
                _targetList.Add(EnemySpawner.instance.specialBossEnemy);
                return;
            }

            // Ÿ���� ������ �켱 Ÿ���ϱ� ������ ���� ���͸� ���� Ž���Ѵ�.
            for (int i = 0; i < EnemySpawner.instance.missionBossEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.missionBossEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                {
                    _targetList.Clear();
                    _targetList.Add(EnemySpawner.instance.missionBossEnemyList[i]);
                    return;
                }
            }

            if (_targetList.Count != 0)
            {
                _distance = Vector3.Distance(_tower.transform.position, _targetList[0].transform.position);
                if (_distance > _tower.range || !_targetList[0].gameObject.activeSelf)
                    _targetList.Clear();
                else
                    return;
            }

            float _closestDistSqr = Mathf.Infinity;

            for (int i = 0; i < EnemySpawner.instance.roundEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.roundEnemyList[i].transform.position);
                if (_distance <= _tower.range && _distance <= _closestDistSqr)
                {
                    _closestDistSqr = _distance;
                    _tempTarget = EnemySpawner.instance.roundEnemyList[i];
                }
            }

            if (_tempTarget != null)
            {
                targetList.Clear();
                targetList.Add(_tempTarget);
                _tempTarget = null;
            }
        }
        // ���� Ÿ�� Ÿ���� ��� ����
        else // (_detectingMode == DetectingMode.Multiple)
        {
            _targetList.Clear();

            // ��Ÿ� ���� ����Ⱥ����� �ִٸ� ���� �켱 Ÿ���Ѵ�.
            _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.specialBossEnemy.transform.position);
            if (_distance <= _tower.range)
                _targetList.Add(EnemySpawner.instance.specialBossEnemy);

            // Ÿ���� ������ �켱 Ÿ���ϱ� ������ ���� ���͸� ���� Ž���Ѵ�.
            for (int i = 0; i < EnemySpawner.instance.missionBossEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.missionBossEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                    _targetList.Add(EnemySpawner.instance.missionBossEnemyList[i]);

                if (_targetList.Count >= _tower.maxTargetCount)
                    break;
            }

            for (int i = 0; i < EnemySpawner.instance.roundEnemyList.Count; i++)
            {
                _distance = Vector3.Distance(_tower.transform.position, EnemySpawner.instance.roundEnemyList[i].transform.position);
                if (_distance <= _tower.range)
                {
                    _targetList.Add(EnemySpawner.instance.roundEnemyList[i]);
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