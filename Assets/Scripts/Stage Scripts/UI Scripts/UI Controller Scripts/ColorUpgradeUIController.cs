using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorUpgradeUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCostTexts;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCountTexts;

    public void SetColorUpgradeCostText(int index, int amount) => _colorUpgradeCostTexts[index].text = amount.ToString() + 'M';
    public void SetColorUpgradeCountText(int index, int amount) => _colorUpgradeCountTexts[index].text = '+' + amount.ToString();
}


/*
 * File : ColorUpgradeUIController.cs
 * First Update : 2022/06/03 FRI 21:10
 * �÷� ���׷��̵� UI�� ��� ����ϴ� ��ũ��Ʈ.
 */