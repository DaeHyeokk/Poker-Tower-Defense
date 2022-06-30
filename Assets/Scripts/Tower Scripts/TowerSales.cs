using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerSales : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _salesGoldText;
    [SerializeField]
    private TextMeshProUGUI _randomTowerRewardText;

    private TowerBuilder _towerBuilder;
    private Tower _tower;

    private void Awake()
    {
        _towerBuilder = FindObjectOfType<TowerBuilder>();
    }
    public void Setup(Tower tower)
    {
        _tower = tower;
        _salesGoldText.text = tower.salesGold.ToString() + 'G';

        if (tower.level >= 3)
            _randomTowerRewardText.gameObject.SetActive(true);
        else
            _randomTowerRewardText.gameObject.SetActive(false);
    } 

    public void SalesTower()
    {
        GameManager.instance.gold += _tower.salesGold;

        if (_randomTowerRewardText.gameObject.activeSelf)
            _towerBuilder.BuildTower(Random.Range(0, 10));

        UIManager.instance.ShowSystemMessage(SystemMessage.MessageType.CompletionTowerSales);
        _tower.ReturnPool();
    }
}
