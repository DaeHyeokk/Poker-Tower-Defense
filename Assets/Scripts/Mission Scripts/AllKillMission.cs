using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllKillMission : Mission
{
    private EnemyCounter _enemyCounter;
    private int _completionCount;
    private readonly string _missionCompletionString = "<color=\"white\">싹쓸이 클리어!</color>";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    public override void CheckMission()
    {
        if (_enemyCounter.roundEnemyCount == 0)
        {
            _completionCount++;
            GiveReward();

        }
    }
}
