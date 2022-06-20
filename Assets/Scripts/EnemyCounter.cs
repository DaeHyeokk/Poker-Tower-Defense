using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private EnemyCounterUIController _enemyCounterUIController;
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
        }
    }

    private void Update()
    {
        roundEnemyCount = _enemySpawner.roundEnemyList.Count;
    }
}
