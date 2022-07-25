using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutItCloseMission : Mission
{
    [SerializeField]
    private TextMeshProUGUI _completionCountText;

    private WaveSystem _waveSystem;
    private int _completionCount;
    private readonly string _missionCompletionString = "<color=\"white\">아슬아슬 클리어!</color>\n";

    protected override string missionCompletionString => _missionCompletionString;

    protected override void Awake()
    {
        base.Awake();
        _waveSystem = FindObjectOfType<WaveSystem>();
    }

    public override void CheckMission()
    {
        if (_waveSystem.isBossWave && _waveSystem.minute == 0 && _waveSystem.second <= 5)
        {
            GiveReward();
            _completionCount++;
            _completionCountText.text = _completionCount.ToString() + "회";
        }
    }
}
