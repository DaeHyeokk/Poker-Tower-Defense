using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDataUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _cardChangeAmountText;
    [SerializeField]
    private TextMeshProUGUI _goldAmountText;
    [SerializeField]
    private TextMeshProUGUI _lifeAmountText;
    [SerializeField]
    private TextMeshProUGUI _mineralAmountText;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCostTexts;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCountTexts;

    public void SetCardChangeAmountText(int amount) => _cardChangeAmountText.text = amount.ToString();
    public void SetGoldAmountText(int amount) => _goldAmountText.text = amount.ToString();
    public void SetLiftAmountText(int amount) => _lifeAmountText.text = amount.ToString();
    public void SetMineralAmountText(int amount) => _mineralAmountText.text = amount.ToString();
    public void SetColorUpgradeCostText(int index, int amount) => _colorUpgradeCostTexts[index].text = amount.ToString() + 'M';
    public void SetColorUpgradeCountText(int index, int amount) => _colorUpgradeCountTexts[index].text = '+' + amount.ToString();
}

/*
 * File : UIManager.cs
 * First Update : 2022/06/02 THU 20:10
 * 게임에서 사용되는 골드, 미네랄, 카드변경, 라이프, 컬러 업그레이드 UI의 제어를 담당하는 스크립트.
 */