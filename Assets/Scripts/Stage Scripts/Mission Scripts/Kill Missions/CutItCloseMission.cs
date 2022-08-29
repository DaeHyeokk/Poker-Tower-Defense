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
    private readonly string _missionCompletionString = "<color=\"white\">�ƽ��ƽ�</color>\n";

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
        // ���� ���尡 �������尡 �ƴϰų�, �ش� �������忡 �̼��� �̹� �Ϸ��� ��� �ǳʶڴ�.
        if (!_waveSystem.isBossWave || _isCompleted) return;

        // ���� ���尡 ���������̰� ������ ���� ������ ��� ����.
        if (_waveSystem.isBossWave && _isBossKilled)
        {
            // ������ ����� �� ���� �ð��� 5�� ���϶�� �̼� ����.
            if (_waveSystem.minute == 0 && _waveSystem.second <= 5)
            {
                CompleteMission();
                _isCompleted = true;
                _isBossKilled = false;
            }
            // ������ ����� �� ���� �ð��� 5�� �̻��̶�� �̼� ����.
            else
                _isBossKilled = false;
        }
    }
}
