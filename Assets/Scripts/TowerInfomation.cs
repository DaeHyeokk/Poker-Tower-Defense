using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfomation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _towerSprite;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _damageText;
    [SerializeField]
    private TextMeshProUGUI _attackRateText;
    [SerializeField]
    private TextMeshProUGUI _colorChangeCostText;
    [SerializeField]
    private TextMeshProUGUI _salePriceText;
    [SerializeField]
    private TextMeshProUGUI _randomTowerRewardText;

    private Tower _fromTower;
}
