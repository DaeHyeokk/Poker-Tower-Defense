using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDefeatUIController : MonoBehaviour
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

        for(int i=0; i<_towerKillCountTexts.Length; i++)
            _towerKillCountTexts[i].text = GameManager.instance.towerKilledCounts[i].ToString() + "킬";
    }
}
