using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private EnemyCounterUIController _enemyCounterUIController;
    [SerializeField]
    private int _maxEnemyCount;

    private int _roundEnemyCount;

    public int roundEnemyCount
    {
        get => _roundEnemyCount;
        set
        {
            if (_roundEnemyCount == value)
                return;

            _roundEnemyCount = value;
            _enemyCounterUIController.SetEnemyCountText(value);

            // ���� ���� �ִ� ���� ���ڿ� ������ ��� �й��Ѵ�.
            if (value >= _maxEnemyCount)
                GameManager.instance.DefeatGame();
        }
    }

    private void Update()
    {
        if (_enemySpawner.roundBossEnemy.gameObject.activeSelf)
            roundEnemyCount = _enemySpawner.roundEnemyList.Count + 1;
        else
            roundEnemyCount = _enemySpawner.roundEnemyList.Count;
    }
}
