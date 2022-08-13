using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllKillMission : RepeatMission
{
    private EnemyCounter _enemyCounter;
    private WaveSystem _waveSystem;
    private bool _isCompleted = true;
    private readonly string _missionCompletionString = "<color=\"white\">싹쓸이</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _waveSystem = FindObjectOfType<WaveSystem>();

        // 새로운 웨이브가 시작될 때 해당 라운드의 미션 완료 여부를 false로 설정하는 이벤트를 등록한다.
        _waveSystem.onWaveStart += () => _isCompleted = false;
    }

    protected override void Update()
    {
        // 해당 라운드에 미션을 이미 완료한 경우 건너뛴다.
        if (_isCompleted) return;

        // 보스웨이브가 아니며 현재 웨이브의 남은 시간이 10초일 때 필드 위에 라운드 몬스터가 0마리라면 미션 성공.
        if (!_waveSystem.isBossWave && _waveSystem.minute == 0 && _waveSystem.second == 10 && _enemyCounter.roundEnemyCount == 0)
        {
            CompleteMission();
            _isCompleted = true;
        }
    }
}
