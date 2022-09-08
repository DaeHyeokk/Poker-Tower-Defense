using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageMenuUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _difficultyText;

    private void OnEnable()
    {
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.stagePauseSound);
        SetDifficultyText();
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
}
