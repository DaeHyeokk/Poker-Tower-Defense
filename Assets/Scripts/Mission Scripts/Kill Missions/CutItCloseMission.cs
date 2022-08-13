using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutItCloseMission : RepeatMission
{
    private WaveSystem _waveSystem;
    private EnemySpawner _enemySpawner;
    private bool _isCompleted = true;
    private readonly string _missionCompletionString = "<color=\"white\">�ƽ��ƽ�</color>\n";

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
        // �ش� �������忡 �̼��� �̹� �Ϸ��� ��� �ǳʶڴ�.
        if (_isCompleted) return;

        // ���� �������̺��̸� ���� �ð��� 5���� �� �������Ͱ� ������� ��� �̼� ����.
        if (_waveSystem.isBossWave && _waveSystem.minute == 0 && _waveSystem.second <= 5 && _enemySpawner.roundBossEnemy.gameObject.activeSelf)
        {
            CompleteMission();
            _isCompleted = true;
        }
    }
}
