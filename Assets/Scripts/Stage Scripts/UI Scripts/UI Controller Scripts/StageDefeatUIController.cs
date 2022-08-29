using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageDefeatUIController : MonoBehaviour
{
    [SerializeField]
    private WaveSystem _waveSystem;
    [SerializeField]
    private TextMeshProUGUI _defeatWaveText;
    [SerializeField]
    private TextMeshProUGUI[] _towerKillCountTexts;

    private void OnEnable()
    {
        _defeatWaveText.text = _waveSystem.wave.ToString();

        for(int towerIndex=0; towerIndex<_towerKillCountTexts.Length; towerIndex++)
            _towerKillCountTexts[towerIndex].text = Tower.GetKillCount(towerIndex).ToString() + "킬";
    }
}
