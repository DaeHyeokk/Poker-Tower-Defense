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
    private RewardStringBuilder _rewardStringBuilder = new();

    private readonly string _soldOutString = "<color=\"white\">판매 완료</color>\n";

    private void Awake()
    {
        _towerBuilder = FindObjectOfType<TowerBuilder>();
    }
    public void Setup(Tower tower)
    {
        _tower = tower;
        _salesGoldText.text = tower.salesGold.ToString() + 'G';
        _rewardStringBuilder.Set(tower.salesGold, 0, 0);

        if (tower.level >= 3)
            _randomTowerRewardText.gameObject.SetActive(true);
        else
            _randomTowerRewardText.gameObject.SetActive(false);
    } 

    public void SalesTower()
    {
        StageManager.instance.gold += _tower.salesGold;

        if (_randomTowerRewardText.gameObject.activeSelf)
            _towerBuilder.BuildTower(Random.Range(0, 10));

        StageUIManager.instance.ShowTowerSalesRewardText(_soldOutString + _rewardStringBuilder.ToString());
        SoundManager.instance.PlaySFX("Tower Sales Sound");
        _tower.ReturnPool();
    }
}
