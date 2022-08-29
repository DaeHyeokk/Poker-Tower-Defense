using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutItCloseMission : RepeatMission
{
    private WaveSystem _waveSystem;
    private EnemySpawner _enemySpawner;
    private bool _isCompleted;
    private bool _isBossKilled;
    private readonly string _missionCompletionString = "<color=\"white\">아슬아슬</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _waveSystem = FindObjectOfType<WaveSystem>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();

        _waveSystem.onBossWaveStart += () => _isCompleted = false;
        _enemySpawner.roundBossEnemy.onDie += () => _isBossKilled = true;
    }

    protected override void Update()
    {
        // 현재 라운드가 보스라운드가 아니거나, 해당 보스라운드에 미션을 이미 완료한 경우 건너뛴다.
        if (!_waveSystem.isBossWave || _isCompleted) return;

        // 현재 라운드가 보스라운드이고 보스를 잡은 상태일 경우 수행.
        if (_waveSystem.isBossWave && _isBossKilled)
        {
            // 보스를 잡았을 때 남은 시간이 5초 이하라면 미션 성공.
            if (_waveSystem.minute == 0 && _waveSystem.second <= 5)
            {
                CompleteMission();
                _isCompleted = true;
                _isBossKilled = false;
            }
            // 보스를 잡았을 때 남은 시간이 5초 이상이라면 미션 실패.
            else
                _isBossKilled = false;
        }
    }
}
