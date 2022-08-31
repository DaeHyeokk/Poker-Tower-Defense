using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField]
    private DifficultySelectUIController _difficultySelectUIController;

    private void Awake()
    {
        LoadPlayerPrefsStageClearData();
    }

    private void LoadPlayerPrefsStageClearData()
    {
        // �÷��̾ ���������� Ŭ������ ����� ����Ǿ� �ִ� ��� ����.
        if (PlayerPrefs.HasKey("Clear Stage"))
        {
            int clearStage = PlayerPrefs.GetInt("Clear Stage");
            _difficultySelectUIController.UnlockDifficultyButton(clearStage);
        }
    }

    public void OnClickEasyButton()
    {
        GameManager.instance.SetStageDifficulty(GameManager.StageDifficulty.Easy);
        SceneManager.LoadScene("SingleStageScene");
    }

    public void OnClickNormalButton()
    {
        GameManager.instance.SetStageDifficulty(GameManager.StageDifficulty.Normal);
        SceneManager.LoadScene("SingleStageScene");
    }

    public void OnClickHardButton()
    {
        GameManager.instance.SetStageDifficulty(GameManager.StageDifficulty.Hard);
        SceneManager.LoadScene("SingleStageScene");
    }

    public void OnClickHellButton()
    {
        GameManager.instance.SetStageDifficulty(GameManager.StageDifficulty.Hell);
        SceneManager.LoadScene("SingleStageScene");
    }

    public void OnClickBackButton()
    {
        _difficultySelectUIController.gameObject.SetActive(false);
    }
}
