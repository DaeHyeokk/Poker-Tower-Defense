using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBossEnemy : FieldBossEnemy
{
    [SerializeField]
    private int _bossLevel;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnMissing()
    {
        GameManager.instance.life -= 10 * _bossLevel;
        ReturnObject();
    }

    protected override void Die()
    {
        base.Die();

        switch (_bossLevel)
        {
            case 1:
                GameManager.instance.gold += 100;
                break;
            case 2:
                GameManager.instance.gold += 200;
                break;
            case 3:
                GameManager.instance.gold += 500;
                break;
        }

        ReturnObject();
    }

    protected override void ReturnObject()
    {
        enemySpawner.missionBossEnemyList.Remove(this);
        this.gameObject.SetActive(false);
    }
}
