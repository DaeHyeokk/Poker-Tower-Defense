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

    private Tower _tower;

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

        if (_randomTowerRewardText.gameObject.activeInHierarchy)
            TowerBuilder.instance.BuildTower(Random.Range(0, 10));

        UIManager.instance.ShowSystemMessage("판매 완료!");
        _tower.ReturnPool();
    }

    private void OnDisable()
    {

    }
}
