using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAllKillMission : NonRepeatMission
{
    private EnemySpawner _enemySpawner;
    private WaveSystem _waveSystem;
    private bool[] _isBossKilleds = new bool[4];
    private readonly string _missionCompletionString = "<color=\"white\">보스 박멸</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
        _waveSystem = FindObjectOfType<WaveSystem>();

        _waveSystem.onBossWaveStart += ResetIsBossKilleds;
        SubscribeActionEvent();
    }

    private void ResetIsBossKilleds()
    {
        for (int i = 0; i < 4; i++)
            _isBossKilleds[i] = false;
    }

    private void SubscribeActionEvent()
    {
        _enemySpawner.missionBossEnemies[0].onDie += () => _isBossKilleds[0] = true;
        _enemySpawner.missionBossEnemies[1].onDie += () => _isBossKilleds[1] = true;
        _enemySpawner.missionBossEnemies[2].onDie += () => _isBossKilleds[2] = true;
        _enemySpawner.roundBossEnemy.onDie += () => _isBossKilleds[3] = true;
    }

    protected override void Update()
    {
        // 미션을 아직 완료하지 않았고 현재 웨이브가 보스 웨이브라면 수행한다.
        if(!isEnd && _waveSystem.isBossWave)
        {
            // 보스를 처치했는지 여부를 나타내는 isBossKilleds 배열을 탐색하여
            // 처치하지 못한 보스가 있을 경우 함수를 즉시 종료한다.
            for (int i = 0; i < 4; i++)
                if (!_isBossKilleds[i])
                    return;

            // 모든 종류의 보스를 처치했다면 미션 성공.
            CompleteMission();
        }
    }
}
