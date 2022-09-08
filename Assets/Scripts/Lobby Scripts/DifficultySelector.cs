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
        PlayerGameData playerGameData = GameManager.instance.playerGameData;
        // 플레이어가 스테이지를 클리어한 기록이 저장되어 있는 경우 수행. (가장 낮은 스테이지 클리어 횟수가 1이상일 경우)
        if (playerGameData.playerStageDataList[(int)StageManager.StageDifficulty.Easy].clearCount > 0)
        {
            int clearStage = (int)StageManager.StageDifficulty.Easy;

            for (int i = 1; i < playerGameData.playerStageDataList.Capacity; i++)
                if (playerGameData.playerStageDataList[i].clearCount > 0)
                    clearStage = i;

            _difficultySelectUIController.UnlockDifficultyButton(clearStage);
        }
    }
    
    public void OnClickEasyButton()
    {
        StageManager.stageDifficulty = StageManager.StageDifficulty.Easy;
        LoadSingleStageScene();
    }

    public void OnClickNormalButton()
    {
        StageManager.stageDifficulty = StageManager.StageDifficulty.Normal;
        LoadSingleStageScene();
    }

    public void OnClickHardButton()
    {
        StageManager.stageDifficulty = StageManager.StageDifficulty.Hard;
        LoadSingleStageScene();
    }

    public void OnClickHellButton()
    {
        StageManager.stageDifficulty = StageManager.StageDifficulty.Hell;
        LoadSingleStageScene();
    }

    public void OnClickReturnButton()
    {
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.popupUIHideSound);
    }

    private void LoadSingleStageScene()
    {
        SceneManager.LoadScene("SingleStageScene");
    }
}
