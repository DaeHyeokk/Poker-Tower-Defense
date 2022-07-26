using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllKillMission : RepeatMission
{
    private EnemyCounter _enemyCounter;
    private readonly string _missionCompletionString = "<color=\"white\">½Ï¾µÀÌ</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
    }

    public override void CheckMission()
    {
        if (_enemyCounter.roundEnemyCount == 0)
            GiveReward();
    }
}
