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
                _difficultyText.text = "���� ���";
                break;

            case StageManager.StageDifficulty.Normal:
                _difficultyText.color = Color.yellow;
                _difficultyText.text = "���� ���";
                break;

            case StageManager.StageDifficulty.Hard:
                _difficultyText.color = new Color(0.6f, 0f, 1f);
                _difficultyText.text = "����� ���";
                break;

            case StageManager.StageDifficulty.Hell:
                _difficultyText.color = Color.red;
                _difficultyText.text = "���� ���";
                break;

            default:
                throw new System.Exception("���̵� ������ ������ �ֽ��ϴ�.");
        }
    }
}
