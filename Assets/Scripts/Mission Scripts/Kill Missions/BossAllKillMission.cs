using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAllKillMission : NonRepeatMission
{
    private EnemySpawner _enemySpawner;
    private WaveSystem _waveSystem;
    private bool[] _isBossKilleds = new bool[4];
    private readonly string _missionCompletionString = "<color=\"white\">���� �ڸ�</color>\n";

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
        // �̼��� ���� �Ϸ����� �ʾҰ� ���� ���̺갡 ���� ���̺��� �����Ѵ�.
        if(!isEnd && _waveSystem.isBossWave)
        {
            // ������ óġ�ߴ��� ���θ� ��Ÿ���� isBossKilleds �迭�� Ž���Ͽ�
            // óġ���� ���� ������ ���� ��� �Լ��� ��� �����Ѵ�.
            for (int i = 0; i < 4; i++)
                if (!_isBossKilleds[i])
                    return;

            // ��� ������ ������ óġ�ߴٸ� �̼� ����.
            CompleteMission();
        }
    }
}
