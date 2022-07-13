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

        // ���� Ÿ�� Ÿ���� ��� ����
        if (_detectingMode == DetectingMode.Single)
        {
            // ����� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� ���� �켱 Ÿ���Ѵ�.
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

            // �״��� ���� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� ���� ������ �켱 Ÿ���Ѵ�.
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

            // �״��� �̼� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� �켱 Ÿ���Ѵ�.
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
            LinkedListNode<FieldEnemy> fieldEnemyNode = _enemySpawner.roundEnemyList.First;
            while (fieldEnemyNode != null)
            {
                distance = Vector2.Distance(_tower.transform.position, fieldEnemyNode.Value.transform.position);

                if (distance <= _tower.range && distance <= _closestDistSqr)
                {
                    _closestDistSqr = distance;
                    tempTarget = fieldEnemyNode.Value;
                }
                fieldEnemyNode = fieldEnemyNode.Next;
            }
            
            if (tempTarget != null)
                targetList.Add(tempTarget);
        }
        // ���� Ÿ�� Ÿ���� ��� ����
        else // (_detectingMode == DetectingMode.Multiple)
        {
            // ���� Ž���ϱ� �� ����Ʈ�� �ʱ�ȭ �Ѵ�.
            _targetList.Clear();

            // ����� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� ���� �켱 Ÿ���Ѵ�.
            if (_enemySpawner.specialBossEnemy.gameObject.activeSelf)
            {
                distance = Vector2.Distance(_tower.transform.position, _enemySpawner.specialBossEnemy.transform.position);
                if (distance <= _tower.range)
                    _targetList.Add(_enemySpawner.specialBossEnemy);
            }

            // �״��� ���� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� ���� ������ �켱 Ÿ���Ѵ�.
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

            // �״��� �̼� ������ �ʵ忡 Ȱ��ȭ �� ���¶�� �̼� ������ �켱 Ÿ���Ѵ�.
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

            LinkedListNode<FieldEnemy> fieldEnemyNode = _enemySpawner.roundEnemyList.First;
            while(fieldEnemyNode != null)
            {
                distance = Vector2.Distance(_tower.transform.position, fieldEnemyNode.Value.transform.position);
                if (distance <= _tower.range)
                    _targetList.Add(fieldEnemyNode.Value);

                if (_targetList.Count >= _tower.maxTargetCount)
                    return;

                fieldEnemyNode = fieldEnemyNode.Next;
            }
        }
    }

    public void SearchTargetWithinRange(Transform centralPosition, float range)
    {
        float distance;
        float rangeRadius = range * 0.5f;

        // ���� Ž���ϱ� �� ����Ʈ�� �ʱ�ȭ �Ѵ�.
        _targetWithinRangeList.Clear();

        // ���� ���� Ȱ��ȭ �� ����Ⱥ����� �ִ��� Ž���Ѵ�.
        if (_enemySpawner.specialBossEnemy.gameObject.activeSelf)
        {
            distance = Vector2.Distance(centralPosition.position, _enemySpawner.specialBossEnemy.transform.position);
            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(_enemySpawner.specialBossEnemy);
        }

        // ���� ���� Ȱ��ȭ �� ���庸���� �ִ��� Ž���Ѵ�.
        if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
        {
            distance = Vector2.Distance(centralPosition.position, _enemySpawner.roundBossEnemy.transform.position);
            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(_enemySpawner.roundBossEnemy);
        }

        // ���� ���� Ȱ��ȭ �� �̼Ǻ����� �ִ��� Ž���Ѵ�.
        for (int i = 0; i < _enemySpawner.missionBossEnemies.Length; i++)
        {
            if (_enemySpawner.missionBossEnemies[i].gameObject.activeSelf)
            {
                distance = Vector2.Distance(centralPosition.position, _enemySpawner.missionBossEnemies[i].transform.position);
                if (distance <= rangeRadius)
                    _targetWithinRangeList.Add(_enemySpawner.missionBossEnemies[i]);
            }
        }

        // ���� ���� ���� ���Ͱ� �ִ��� Ž���Ѵ�.
        LinkedListNode<FieldEnemy> fieldEnemyNode = _enemySpawner.roundEnemyList.First;
        while(fieldEnemyNode != null)
        {
            distance = Vector2.Distance(centralPosition.position, fieldEnemyNode.Value.transform.position);

            if (distance <= rangeRadius)
                _targetWithinRangeList.Add(fieldEnemyNode.Value);

            fieldEnemyNode = fieldEnemyNode.Next;
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