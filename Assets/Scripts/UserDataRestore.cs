using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataRestore : MonoBehaviour
{
    private float _easyRecord = 5.689f;
    private float _normalRecord = 54.235f;

    private void Awake()
    {
        if(Social.localUser.authenticated && Social.localUser.userName == "Puzzlemaster6744")
            if(!GameManager.instance.playerGameData.isDataRestored)
                RestoreData();
    }

    private void RestoreData()
    {
        PlayerStageData easyStageData = GameManager.instance.playerGameData.playerStageDataList[(int)StageManager.StageDifficulty.Easy];
        PlayerStageData normalStageData = GameManager.instance.playerGameData.playerStageDataList[(int)StageManager.StageDifficulty.Normal];

        easyStageData.clearCount++;
        easyStageData.bestRoundRecord = 40;
        if (easyStageData.bestBossKilledTakenTimeRecord > _easyRecord)
            easyStageData.bestBossKilledTakenTimeRecord = _easyRecord;

        normalStageData.clearCount++;
        normalStageData.bestRoundRecord = 40;
        if (normalStageData.bestBossKilledTakenTimeRecord > _normalRecord)
            normalStageData.bestBossKilledTakenTimeRecord = _normalRecord;

        GameManager.instance.playerGameData.playerStageDataList[(int)StageManager.StageDifficulty.Easy] = easyStageData;
        GameManager.instance.playerGameData.playerStageDataList[(int)StageManager.StageDifficulty.Normal] = normalStageData;

        Tower.AddPlayerTowerKillCount((int)Tower.TowerTypeEnum.풀하우스타워, 6000);

        GameManager.instance.playerGameData.isDataRestored = true;
        GameManager.instance.Save();
    }
}
