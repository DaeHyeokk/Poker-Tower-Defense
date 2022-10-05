using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTowerInfoUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _levelTexts;

    [SerializeField]
    private TextMeshProUGUI[] _damageTexts;
    [SerializeField]
    private TextMeshProUGUI[] _increaseDamageTexts;

    [SerializeField]
    private TextMeshProUGUI[] _upgradeDIPTexts;
    [SerializeField]
    private TextMeshProUGUI[] _increaseUpgradeDIPTexts;

    [SerializeField]
    private TextMeshProUGUI[] _attackRateTexts;
    [SerializeField]
    private TextMeshProUGUI[] _increaseAttackRateTexts;

    [SerializeField]
    private TextMeshProUGUI[] _killCountTexts;
    [SerializeField]
    private TextMeshProUGUI[] _nextLevelKillCountTexts;
    [SerializeField]
    private Slider[] _killCountSliders;

    [SerializeField]
    private TowerData[] _towerDatas;

    private void OnEnable()
    {
        SetPlayerTowerInfo();
    }

    private void SetPlayerTowerInfo()
    {
        for(int i=0; i<Tower.towerTypeNames.Length; i++)
        {
            PlayerTowerData playerTowerData = GameManager.instance.playerGameData.playerTowerDataList[i];

            int level = playerTowerData.level;
            int killCount = playerTowerData.killCount;
            int nextLevelKillCount = level * Tower.defaultLevelupKillCount;

            float increaseDamage = _towerDatas[i].levelup.damage;
            float damage = increaseDamage * (level - 1);

            float increaseUpgradeDIP = _towerDatas[i].levelup.upgradeDIP;
            float upgradeDIP = increaseUpgradeDIP * (level - 1);

            float increaseAttackRate = _towerDatas[i].levelup.rate;
            float attackRate = increaseAttackRate * (level - 1);

            _levelTexts[i].text = "Lv " + level.ToString();

            _damageTexts[i].text = damage.ToString();
            _increaseDamageTexts[i].text = "+" + increaseDamage.ToString();

            _upgradeDIPTexts[i].text = upgradeDIP.ToString();
            _increaseUpgradeDIPTexts[i].text = "+" + increaseUpgradeDIP.ToString();

            _attackRateTexts[i].text = attackRate.ToString();
            _increaseAttackRateTexts[i].text = "+" + increaseAttackRate.ToString();

            _killCountTexts[i].text = killCount.ToString();
            _nextLevelKillCountTexts[i].text = nextLevelKillCount.ToString();

            _killCountSliders[i].maxValue = nextLevelKillCount;
            _killCountSliders[i].value = killCount;
        }
    }
}
