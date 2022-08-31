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
        SetDifficultyText();
    }

    private void SetDifficultyText()
    {
        switch (GameManager.instance.stageDifficulty)
        {
            case GameManager.StageDifficulty.Easy:
                _difficultyText.color = Color.green;
                _difficultyText.text = "���� ���";
                break;

            case GameManager.StageDifficulty.Normal:
                _difficultyText.color = Color.yellow;
                _difficultyText.text = "���� ���";
                break;

            case GameManager.StageDifficulty.Hard:
                _difficultyText.color = new Color(0.6f, 0f, 1f);
                _difficultyText.text = "����� ���";
                break;

            case GameManager.StageDifficulty.Hell:
                _difficultyText.color = Color.red;
                _difficultyText.text = "���� ���";
                break;

            default:
                throw new System.Exception("���̵� ������ ������ �ֽ��ϴ�.");
        }
    }
}
