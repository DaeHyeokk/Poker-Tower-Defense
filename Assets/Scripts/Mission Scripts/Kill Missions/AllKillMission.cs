using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllKillMission : RepeatMission
{
    private EnemyCounter _enemyCounter;
    private WaveSystem _waveSystem;
    private bool _isCompleted = true;
    private readonly string _missionCompletionString = "<color=\"white\">�Ͼ���</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _enemyCounter = FindObjectOfType<EnemyCounter>();
        _waveSystem = FindObjectOfType<WaveSystem>();

        // ���ο� ���̺갡 ���۵� �� �ش� ������ �̼� �Ϸ� ���θ� false�� �����ϴ� �̺�Ʈ�� ����Ѵ�.
        _waveSystem.onWaveStart += () => _isCompleted = false;
    }

    protected override void Update()
    {
        // �ش� ���忡 �̼��� �̹� �Ϸ��� ��� �ǳʶڴ�.
        if (_isCompleted) return;

        // �������̺갡 �ƴϸ� ���� ���̺��� ���� �ð��� 10���� �� �ʵ� ���� ���� ���Ͱ� 0������� �̼� ����.
        if (!_waveSystem.isBossWave && _waveSystem.minute == 0 && _waveSystem.second == 10 && _enemyCounter.roundEnemyCount == 0)
        {
            CompleteMission();
            _isCompleted = true;
        }
    }
}
