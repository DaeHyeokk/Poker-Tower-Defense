using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfomation : MonoBehaviour
{
    [SerializeField]
    private TowerAbilityUIController _towerAbilityUIController;
    [SerializeField]
    private TowerSales _towerSales;
    [SerializeField]
    private TowerColorChanger _towerColorChanger;
    [SerializeField]
    private TowerDetailInfo _towerDetailInfo;

    public void Setup(Tower tower)
    {
        _towerAbilityUIController.Setup(tower);
        _towerSales.Setup(tower);
        _towerColorChanger.Setup(tower);
        _towerDetailInfo.Setup(tower);
    }
}
