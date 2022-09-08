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
    [SerializeField]
    private GameObject _rewardBonusTextObject;

    private void Awake()
    {
        if (GameManager.instance.playerGameData.isBonusRewardActived)
            _rewardBonusTextObject.SetActive(true);
        else
            _rewardBonusTextObject.SetActive(false);

        SetDifficultyText();
        SetTowerKillCountText();

        _clearCountText.text = StageManager.instance.clearCount.ToString() + "회";
    }

    private void SetDifficultyText()
    {
        switch (StageManager.stageDifficulty)
        {
            case StageManager.StageDifficulty.Easy:
                _difficultyText.color = Color.green;
                _difficultyText.text = "쉬움 모드";
                break;

            case StageManager.StageDifficulty.Normal:
                _difficultyText.color = Color.yellow;
                _difficultyText.text = "보통 모드";
                break;

            case StageManager.StageDifficulty.Hard:
                _difficultyText.color = new Color(0.6f, 0f, 1f);
                _difficultyText.text = "어려움 모드";
                break;

            case StageManager.StageDifficulty.Hell:
                _difficultyText.color = Color.red;
                _difficultyText.text = "지옥 모드";
                break;

            default:
                throw new System.Exception("난이도 설정에 오류가 있습니다.");
        }
    }
    private void SetTowerKillCountText()
    {
        for (int towerIndex = 0; towerIndex < _towerKillCountTexts.Length; towerIndex++)
        {
            int towerKillsCount = Tower.GetKillCount(towerIndex) * 2 * (GameManager.instance.playerGameData.isBonusRewardActived ? 2 : 1);
            _towerKillCountTexts[towerIndex].text = Tower.GetKillCount(towerIndex).ToString() + "킬";
        }
    }
}
