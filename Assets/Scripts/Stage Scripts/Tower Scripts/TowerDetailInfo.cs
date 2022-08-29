using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetailInfo : MonoBehaviour
{
    [SerializeField]
    private TowerDetailInfoUIController _towerDetailInfoUIController;

    private Tower _tower;

    public void Setup(Tower tower)
    {
        _tower = tower;
    }

    public void ShowTowerDetailInfo()
    {
        _towerDetailInfoUIController.Setup(_tower);
        _towerDetailInfoUIController.gameObject.SetActive(true);
        //StageUIManager.instance.ShowTowerDetailInfo();
    }
}
