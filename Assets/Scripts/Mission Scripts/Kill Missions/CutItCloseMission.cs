using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutItCloseMission : RepeatMission
{
    private WaveSystem _waveSystem;
    private EnemySpawner _enemySpawner;
    private bool _isCompleted = true;
    private readonly string _missionCompletionString = "<color=\"white\">아슬아슬</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _waveSystem = FindObjectOfType<WaveSystem>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();

        _waveSystem.onBossWaveStart += () => _isCompleted = false;
    }

    protected override void Update()
    {
        // 해당 보스라운드에 미션을 이미 완료한 경우 건너뛴다.
        if (_isCompleted) return;

        // 현재 보스웨이브이며 남은 시간이 5초일 때 보스몬스터가 살아있을 경우 미션 성공.
        if (_waveSystem.isBossWave && _waveSystem.minute == 0 && _waveSystem.second <= 5 && _enemySpawner.roundBossEnemy.gameObject.activeSelf)
        {
            CompleteMission();
            _isCompleted = true;
        }
    }
}
