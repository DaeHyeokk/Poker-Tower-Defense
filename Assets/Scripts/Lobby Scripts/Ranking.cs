using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _bestTakenTimeRecordTexts;

    private void OnEnable()
    {
        for(int i=0; i< _bestTakenTimeRecordTexts.Length; i++)
        {
            float bestBossKilledTakenTimeRecord = GameManager.instance.playerGameData.playerStageDataList[i].bestBossKilledTakenTimeRecord;
            if (bestBossKilledTakenTimeRecord != PlayerStageData.DEFAULT_TAKEN_TIME)
                _bestTakenTimeRecordTexts[i].text = bestBossKilledTakenTimeRecord.ToString("F3") + "ÃÊ";
            else
                _bestTakenTimeRecordTexts[i].text = "¾øÀ½";
        }
    }

    public void OnClickDifficultyRankingButton(int index)
    {
        if (index == (int)StageManager.StageDifficulty.Easy)
            GameManager.instance.ShowBossKilledTakenTimeLeaderboard(StageManager.StageDifficulty.Easy);

        else if (index == (int)StageManager.StageDifficulty.Normal)
            GameManager.instance.ShowBossKilledTakenTimeLeaderboard(StageManager.StageDifficulty.Normal);

        else if (index == (int)StageManager.StageDifficulty.Hard)
            GameManager.instance.ShowBossKilledTakenTimeLeaderboard(StageManager.StageDifficulty.Hard);

        else if (index == (int)StageManager.StageDifficulty.Hell)
            GameManager.instance.ShowBossKilledTakenTimeLeaderboard(StageManager.StageDifficulty.Hell);
    }
}
