using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageClearUIController : MonoBehaviour
{
    [Serializable]
    private struct RecordBossKilledTakenTimePanel
    {
        public GameObject recordPanelObject;
        public TextMeshProUGUI bossKilledTakenTimetext;
        public TextMeshProUGUI bestBossKilledTakenTimeText;
    }
    [Serializable]
    private struct NewRecordBossKilledtakenTimePanel
    {
        public GameObject newRecordPanelObject;
        public TextMeshProUGUI newRecordBossKilledTakenTimeText;
    }

    [SerializeField]
    private RecordBossKilledTakenTimePanel _recordBossKilledTakenTime;
    [SerializeField]
    private NewRecordBossKilledtakenTimePanel _newRecordBossKilledtakenTimePanel;
    [SerializeField]
    private TextMeshProUGUI _difficultyText;
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
        SetBossKilledTakenTimeText();
        SetTowerKillCountText();
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

    private void SetBossKilledTakenTimeText()
    {
        float bossKilledTakenTime = StageManager.instance.bossKilledTakenTime;
        float bestBossKilledTakenTimeRecord = StageManager.instance.bestBossKilledTakenTimeRecord;

        // 현재 스테이지에서 기록한 보스 처치 시간이 최고 기록보다 짧다면 최고 기록 갱신 오브젝트를 활성화 한다.
        if(bossKilledTakenTime < bestBossKilledTakenTimeRecord)
        {
            _recordBossKilledTakenTime.recordPanelObject.SetActive(false);
            _newRecordBossKilledtakenTimePanel.newRecordPanelObject.SetActive(true);
            _newRecordBossKilledtakenTimePanel.newRecordBossKilledTakenTimeText.text = bossKilledTakenTime.ToString("F3") + "초";
        }
        else
        {
            _newRecordBossKilledtakenTimePanel.newRecordPanelObject.SetActive(false);
            _recordBossKilledTakenTime.recordPanelObject.SetActive(true);
            _recordBossKilledTakenTime.bossKilledTakenTimetext.text = bossKilledTakenTime.ToString("F3") + "초";
            _recordBossKilledTakenTime.bestBossKilledTakenTimeText.text = bestBossKilledTakenTimeRecord.ToString("F3") + "초";
        }
    }

    private void SetTowerKillCountText()
    {
        for (int towerIndex = 0; towerIndex < _towerKillCountTexts.Length; towerIndex++)
        {
            int towerKillsCount = StageManager.instance.GetStageTowerKillCount(towerIndex, true);
            _towerKillCountTexts[towerIndex].text = Tower.GetKillCount(towerIndex).ToString() + "킬";
        }
    }
}
