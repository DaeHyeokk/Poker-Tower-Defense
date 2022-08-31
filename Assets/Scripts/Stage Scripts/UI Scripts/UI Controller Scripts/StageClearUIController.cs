using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageClearUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _difficultyText;
    [SerializeField]
    private TextMeshProUGUI _clearCountText;
    [SerializeField]
    private TextMeshProUGUI[] _towerKillCountTexts;

    private void OnEnable()
    {
        SetDifficultyText();

        _clearCountText.text = StageManager.instance.clearCount.ToString() + "회";

        for(int towerIndex=0; towerIndex<_towerKillCountTexts.Length; towerIndex++)
            _towerKillCountTexts[towerIndex].text = Tower.GetKillCount(towerIndex).ToString() + "킬";
    }

    private void SetDifficultyText()
    {
        switch (GameManager.instance.stageDifficulty)
        {
            case GameManager.StageDifficulty.Easy:
                _difficultyText.color = Color.green;
                _difficultyText.text = "쉬움 모드";
                break;

            case GameManager.StageDifficulty.Normal:
                _difficultyText.color = Color.yellow;
                _difficultyText.text = "보통 모드";
                break;

            case GameManager.StageDifficulty.Hard:
                _difficultyText.color = new Color(0.6f, 0f, 1f);
                _difficultyText.text = "어려움 모드";
                break;

            case GameManager.StageDifficulty.Hell:
                _difficultyText.color = Color.red;
                _difficultyText.text = "지옥 모드";
                break;

            default:
                throw new System.Exception("난이도 설정에 오류가 있습니다.");
        }
    }
}
