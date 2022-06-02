using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerAbilityUI : MonoBehaviour
{
    [SerializeField]
    private Image _towerImage;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _damageText;
    [SerializeField]
    private TextMeshProUGUI _attackRateText;

    private Tower _tower;

    public void Setup(Tower tower)
    {
        _tower = tower;
        _towerImage.sprite = tower.towerRenderer.sprite;
        _towerImage.color = tower.towerColor.color;
        _nameText.text = tower.towerName;
        // �Ҽ��� ù° �ڸ����� �ݿø�
        _damageText.text = Mathf.Round(tower.damage).ToString();
        // �Ҽ��� ��° �ڸ����� �ݿø�
        _attackRateText.text = Math.Round(tower.attackRate, 2).ToString();
    }
}
