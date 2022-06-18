using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
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

            if (value > 60)
                UIManager.instance.BlinkScreen(Color.red, 4f);
            else
                UIManager.instance.StopBlinkScreen();
        }
    }

    private void Update()
    {
        roundEnemyCount = EnemySpawner.instance.roundEnemyList.Count;
    }
}
