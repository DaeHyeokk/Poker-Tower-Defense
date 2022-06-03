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
    
    private Tower _tower;

    public Tower tower => _tower;

    public void Setup(Tower tower)
    {
        _towerAbilityUIController.Setup(tower);
        _towerSales.Setup(tower);
        _towerColorChanger.Setup(tower);
    }
}
