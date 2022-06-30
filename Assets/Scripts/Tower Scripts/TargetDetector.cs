using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector
{
    public enum DetectingMode { Single, Multiple }

    private DetectingMode _detectingMode;
    private Tower _tower;
    private EnemySpawner _enemySpawner;
    private List<Enemy> _targetList = new();
    private List<Enemy> _targetWithinRangeList = new();

    public DetectingMode detectingMode { set => _detectingMode = value; }
    public List<Enemy> targetList => _targetList;
    public List<Enemy> targetWithinRangeList => _targetWithinRangeList;

    public TargetDetector(Tower tower, EnemySpawner enemySpawner)
    {
        _tower = tower;

        _enemySpawner = enemySpawner;
    }

    public void SearchTarget()
    {
        float distance;

        // 단일 타겟 타워일 경우 수행
        if (_detectingMode == DetectingMode.Single)
        {
            // 스폐셜 보스가 필드에 활성화 된 상태라면 가장 우선 타격한다.
            if (_enemySpawner.specialBossEnemy.gameObject.activeSelf)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.specialBossEnemy.transform.position);
                if (distance <= _tower.range)
                {
                    _targetList.Clear();
                    _targetList.Add(_enemySpawner.specialBossEnemy);
                    return;
                }
            }

            // 그다음 라운드 보스가 필드에 활성화 된 상태라면 라운드 보스를 우선 타격한다.
            if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.roundBossEnemy.transform.position);
                if (distance <= _tower.range)
                { 
                    _targetList.Clear();
                    _targetList.Add(_enemySpawner.roundBossEnemy);
                    return;
                }
            }

            // 그다음 미션 보스가 필드에 활성화 된 상태라면 우선 타격한다.
            for (int i = 0; i < _enemySpawner.missionBossEnemies.Length; i++)
            {
                if (_enemySpawner.missionBossEnemies[i].gameObject.activeSelf)
                {
                    distance = Vector2.Distance(_tower.transform.position, _enemySpawner.missionBossEnemies[i].transform.position);
                    if (distance <= _tower.range)
                    {
                        _targetList.Clear();
                        _targetList.Add(_enemySpawner.missionBossEnemies[i]);
                        return;
                    }
                }
            }

            if (_targetList.Count > 0)
            {
                distance = Vector2.Distance(_tower.transform.position, _targetList[0].transform.position);
                if (!_targetList[0].gameObject.activeSelf || distance > _tower.range)
                    _targetList.Clear();
                else
                    return;
            }

            float _closestDistSqr = Mathf.Infinity;
            Enemy tempTarget = null;
            for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.roundEnemyList[i].transform.position);

                if (distance <= _tower.range && distance <= _closestDistSqr)
                {
                    _closestDistSqr = distance;
                    tempTarget = _enemySpawner.roundEnemyList[i];
                }
            }
            
            if (tempTarget != null)
            {
                targetList.Add(tempTarget);
                tempTarget = null;
            }
        }
        // 다중 타겟 타워일 경우 수행
        else // (_detectingMode == DetectingMode.Multiple)
        {
            // 적을 탐색하기 전 리스트를 초기화 한다.
            _targetList.Clear();

            // 스폐셜 보스가 필드에 활성화 된 상태라면 가장 우선 타격한다.
            if (_enemySpawner.specialBossEnemy.gameObject.activeSelf)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.specialBossEnemy.transform.position);
                if (distance <= _tower.range)
                    _targetList.Add(_enemySpawner.specialBossEnemy);
            }

            // 그다음 라운드 보스가 필드에 활성화 된 상태라면 라운드 보스를 우선 타격한다.
            if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.roundBossEnemy.transform.position);
                if (distance <= _tower.range)
                {
                    _targetList.Add(_enemySpawner.roundBossEnemy);

                    if (_targetList.Count >= _tower.maxTargetCount)
                        return;
                }
            }

            // 그다음 미션 보스가 필드에 활성화 된 상태라면 미션 보스를 우선 타격한다.
            for (int i = 0; i < _enemySpawner.missionBossEnemies.Length; i++)
            {
                if (_enemySpawner.missionBossEnemies[i].gameObject.activeSelf)
                {
                    distance = Vector2.Distance(_tower.transform.position, _enemySpawner.missionBossEnemies[i].transform.position);
                    if (distance <= _tower.range)
                        _targetList.Add(_enemySpawner.missionBossEnemies[i]);

                    if (_targetList.Count >= _tower.maxTargetCount)
                        return;
                }
            }

            for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.roundEnemyList[i].transform.position);
                if (distance <= _tower.range)
                    _targetList.Add(_enemySpawner.roundEnemyList[i]);

                if (_targetList.Count >= _tower.maxTargetCount)
                    return;
            }
        }
    }

    public void SearchTargetWithinRange(Transform centralPosition, float range)
    {
        float distance;
        float rangeRadius = range * 0.5f;

        // 적을 탐색하기 전 리스트를 초기화 한다.
        _targetWithinRangeList.Clear();

        // 범위 내에 활성화 된 스폐셜보스가 있는지 탐색한다.
        if (_enemySpawner.specialBossEnemy.gameObject.activeSelf)
        {
            distance = Vector2.Distance(centralPosition.position, _enemySpawner.specialBossEnemy.transform.position);
            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(_enemySpawner.specialBossEnemy);
        }

        // 범위 내에 활성화 된 라운드보스가 있는지 탐색한다.
        if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
        {
            distance = Vector2.Distance(centralPosition.position, _enemySpawner.roundBossEnemy.transform.position);
            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(_enemySpawner.roundBossEnemy);
        }

        // 범위 내에 활성화 된 미션보스가 있는지 탐색한다.
        for (int i = 0; i < _enemySpawner.missionBossEnemies.Length; i++)
        {
            if (_enemySpawner.missionBossEnemies[i].gameObject.activeSelf)
            {
                distance = Vector2.Distance(centralPosition.position, _enemySpawner.missionBossEnemies[i].transform.position);
                if (distance <= rangeRadius)
                    _targetWithinRangeList.Add(_enemySpawner.missionBossEnemies[i]);
            }
        }

        // 범위 내에 라운드 몬스터가 있는지 탐색한다.
        for (int i = 0; i < _enemySpawner.roundEnemyList.Count; i++)
        {
            distance = Vector2.Distance(centralPosition.position, _enemySpawner.roundEnemyList[i].transform.position);

            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(_enemySpawner.roundEnemyList[i]);
        }
    }

    public void ResetTarget()
    {
        _targetList.Clear();
        _targetWithinRangeList.Clear();
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