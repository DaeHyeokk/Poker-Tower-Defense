using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerColorChanger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _colorChangeCostText;

    private Tower _tower;
    private int _changeCost;

    public void Setup(Tower tower)
    {
        _tower = tower;
        _changeCost = 50 * (int)Mathf.Pow(2, tower.level);
        _colorChangeCostText.text = _changeCost.ToString() + 'G';
    }

    public void ChangeColor()
    {
        if(GameManager.instance.gold > _changeCost)
        {
            UIManager.instance.ShowSystemMessage("골드가 부족합니다");
            return;
        }

        _tower.towerColor.ChangeColor();
    }
}
