using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageClearUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _gameDifficultyText;
    [SerializeField]
    private TextMeshProUGUI _clearCountText;
    [SerializeField]
    private TextMeshProUGUI[] _towerKillCountTexts;

    private void OnEnable()
    {
        for(int towerIndex=0; towerIndex<_towerKillCountTexts.Length; towerIndex++)
            _towerKillCountTexts[towerIndex].text = Tower.GetKillCount(towerIndex).ToString() + "킬";
    }
}
