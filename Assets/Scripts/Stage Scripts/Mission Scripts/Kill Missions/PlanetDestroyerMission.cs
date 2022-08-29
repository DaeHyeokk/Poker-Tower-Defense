using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetDestroyerMission : NonRepeatMission
{
    private WaveSystem _waveSystem;
    private EnemySpawner _enemySpawner;
    private readonly string _missionCompletionString = "<color=\"white\">행성파괴자</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _waveSystem = FindObjectOfType<WaveSystem>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    protected override void Update()
    {
        // 미션이 종료된 상태라면 건너뛴다.
        if (isEnd) return;

        // 현재 웨이브가 10 이하이며 스폐셜보스 레벨이 4라면 미션 성공.
        if (_waveSystem.wave <= 10 && _enemySpawner.specialBossEnemy.level == 4)
            CompleteMission();
        // 현재 웨이브가 10웨이브를 넘게될 경우 미션 실패.
        else if (_waveSystem.wave > 10)
            FailedMission();
    }
}
