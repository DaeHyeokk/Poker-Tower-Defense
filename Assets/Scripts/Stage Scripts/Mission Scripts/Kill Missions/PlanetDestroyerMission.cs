using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetDestroyerMission : NonRepeatMission
{
    private WaveSystem _waveSystem;
    private EnemySpawner _enemySpawner;
    private readonly string _missionCompletionString = "<color=\"white\">�༺�ı���</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _waveSystem = FindObjectOfType<WaveSystem>();
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    protected override void Update()
    {
        // �̼��� ����� ���¶�� �ǳʶڴ�.
        if (isEnd) return;

        // ���� ���̺갡 10 �����̸� ����Ⱥ��� ������ 4��� �̼� ����.
        if (_waveSystem.wave <= 10 && _enemySpawner.specialBossEnemy.level == 4)
            CompleteMission();
        // ���� ���̺갡 10���̺긦 �ѰԵ� ��� �̼� ����.
        else if (_waveSystem.wave > 10)
            FailedMission();
    }
}
