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
    private TextMeshProUGUI _mineralAmountText;
    [SerializeField]
    private TextMeshProUGUI _gameSpeedText;

    public void SetCardChangeAmountText(int amount) => _cardChangeAmountText.text = amount.ToString();
    public void SetGoldAmountText(int amount) => _goldAmountText.text = amount.ToString();
    public void SetMineralAmountText(int amount) => _mineralAmountText.text = amount.ToString();
    public void SetGameSpeedText(float speed) => _gameSpeedText.text = 'x' + speed.ToString();
}

/*
 * File : GameDataUIController.cs
 * First Update : 2022/06/02 THU 20:10
 * 게임에서 사용되는 골드, 미네랄, 카드변경, 라이프 UI의 제어를 담당하는 스크립트.
 * 
 * Update : 2022/06/16 THU 19:30
 * 라이프 UI 제거.
 */