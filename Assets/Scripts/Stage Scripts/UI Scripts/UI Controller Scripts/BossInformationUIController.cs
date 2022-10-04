using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossInformationUIController : MonoBehaviour
{
    public enum BossType { RoundBoss, MissionBoss, SpecialBoss }

    [System.Serializable]
    public struct BossDetailInfoPanel
    {
        public GameObject bossDetailInfoPanelObject;
        public TextMeshProUGUI bossNameText;
        public Image bossImage;
        public TextMeshProUGUI bossMaxHealthText;
        public TextMeshProUGUI bossRewardText;
    }

    [SerializeField]
    private Image[] _roundBossImages;
    [SerializeField]
    private Image[] _missionBossImages;
    [SerializeField]
    private Image[] _specialBossImages;

    [SerializeField]
    private GameObject[] _bossPanelObjects;

    [SerializeField]
    private BossDetailInfoPanel _bossDetailInfoPanel;

    [SerializeField]
    private EnemySpawner _enemySpawner;

    private RewardStringBuilder _rewardStringBuilder = new();
    private int _bossPanelIndex;

    private void Awake()
    {
        // Boss Information의 보스 이미지를 EnemyData를 참조하여 대입한다.
        for (int i = 0; i < _roundBossImages.Length; i++)
            _roundBossImages[i].sprite = _enemySpawner.roundBossEnemyDatas[i].sprite;

        for (int i = 0; i < _missionBossImages.Length; i++)
            _missionBossImages[i].sprite = _enemySpawner.missionBossEnemyDatas[i].sprite;

        for (int i = 0; i < _specialBossImages.Length; i++)
            _specialBossImages[i].sprite = _enemySpawner.specialBossEnemyDatas[i].sprite;
    }

    private void OnEnable()
    {
        _bossPanelIndex = 0;
        
        for(int i=0; i<_bossPanelObjects.Length; i++)
        {
            if (i == 0) 
                _bossPanelObjects[i].SetActive(true);
            else 
                _bossPanelObjects[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        _bossDetailInfoPanel.bossDetailInfoPanelObject.SetActive(false);
    }

    public void SetBossDetailInfoUI(BossType bossType, int index)
    {
        BossEnemyData bossEnemyData;

        switch(bossType)
        {
            case BossType.RoundBoss:
                if (index >= _enemySpawner.roundBossEnemyDatas.Length)
                    throw new System.Exception("Round Boss Index 범위 초과");
                bossEnemyData = _enemySpawner.roundBossEnemyDatas[index];
                break;

            case BossType.MissionBoss:
                if (index >= _enemySpawner.missionBossEnemyDatas.Length)
                    throw new System.Exception("Mission Boss Index 범위 초과");
                bossEnemyData = _enemySpawner.missionBossEnemyDatas[index];
                break;

            case BossType.SpecialBoss:
                if (index >= _enemySpawner.specialBossEnemyDatas.Length)
                    throw new System.Exception("Special Boss Index 범위 초과");
                bossEnemyData = _enemySpawner.specialBossEnemyDatas[index];
                break;

            default:
                throw new System.Exception("Boss Type 매개변수가 올바르지 않음");
        }

        _bossDetailInfoPanel.bossNameText.text = bossEnemyData.bossName;
        _bossDetailInfoPanel.bossImage.sprite = bossEnemyData.sprite;

        if (bossType == BossType.SpecialBoss)
            _bossDetailInfoPanel.bossMaxHealthText.text = ((int)Mathf.Round(bossEnemyData.health * StageManager.instance.specialBossHpPercentage)).ToString();
        else
            _bossDetailInfoPanel.bossMaxHealthText.text = ((int)Mathf.Round(bossEnemyData.health * StageManager.instance.bossEnemyHpPercentage)).ToString();

        _rewardStringBuilder.Set(bossEnemyData.rewardGold, bossEnemyData.rewardChangeChance, bossEnemyData.rewardTowerLevel);

        if (bossType == BossType.RoundBoss && index == _roundBossImages.Length - 1)
            _bossDetailInfoPanel.bossRewardText.text = "게임 클리어";
        else
            _bossDetailInfoPanel.bossRewardText.text = _rewardStringBuilder.ToString();
    }

    public void OnClickPageNextButton()
    {
        _bossPanelIndex++;

        if (_bossPanelIndex >= _bossPanelObjects.Length)
            _bossPanelIndex = 0;

        for (int i = 0; i < _bossPanelObjects.Length; i++)
        {
            if (i == _bossPanelIndex)
                _bossPanelObjects[i].SetActive(true);
            else
                _bossPanelObjects[i].SetActive(false);
        }
    }

    public void OnClickPagePrevButton()
    {
        _bossPanelIndex--;

        if (_bossPanelIndex < 0)
            _bossPanelIndex = _bossPanelObjects.Length - 1;

        for (int i = 0; i < _bossPanelObjects.Length; i++)
        {
            if (i == _bossPanelIndex)
                _bossPanelObjects[i].SetActive(true);
            else
                _bossPanelObjects[i].SetActive(false);
        }
    }

    public void ShowBossDetailInfoPanel()
    {
        _bossDetailInfoPanel.bossDetailInfoPanelObject.SetActive(true);
    }

    public void HideBossDetailInfoPanel()
    {
        _bossDetailInfoPanel.bossDetailInfoPanelObject.SetActive(false);
    }
}
