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
        if (_targetList.Count != 0)
        {
            int index = 0;
            while (index < _targetList.Count)
            {
                _distance = Vector3.Distance(_tower.transform.position, _targetList[index].transform.position);
                if (_distance > _tower.range)
                {
                    _targetList.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        if (_targetList.Count < _tower.maxTargetCount)
        {
            if (_detectingMode == DetectingMode.Single)
            {
                float _closestDistSqr = Mathf.Infinity;

                for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
                {
                    _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.enemyList[i].transform.position);
                    if (_distance <= _tower.range && _distance <= _closestDistSqr)
                    {
                        _closestDistSqr = _distance;
                        _tempTarget = _enemySpawner.enemyList[i];
                    }
                }

                if (_tempTarget != null)
                {
                    targetList.Add(_tempTarget);
                    _tempTarget = null;
                }
            }

            else // (_detectingMode == DetectingMode.Multiple)
            {
                for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
                {
                    _distance = Vector3.Distance(_tower.transform.position, _enemySpawner.enemyList[i].transform.position);
                    if (_distance <= _tower.range)
                    {
                        _targetList.Add(_enemySpawner.enemyList[i]);
                    }

                    if (_targetList.Count >= _tower.maxTargetCount)
                        break;
                }
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
 * Tower의 타겟 탐지 기능을 담당하는 스크립트.
 * EnemySpawner 컴포넌트를 통해 살아있는 Enemy 오브젝트들의 정보를 가져오고, 
 * TowerWeapon 컴포넌트를 통해 얻은 타워의 공격 범위 내에 있는 Enemy 오브젝트를 찾도록 구현.
 * 
 * Update : 2022/05/01 SUM 15:30
 * 타겟이 사거리를 벗어나는 상황 외에 타겟이 죽거나, 골인 지점에 들어가는 상황일 때도 Target을 Reset하는 로직 추가.
 * 
 * Update : 2022/05/03 THU 03:13
 * 타워의 사거리를 플레이어에게 보여주는 로직 추가.
 */