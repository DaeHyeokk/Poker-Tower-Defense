using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerAbilityUIController : MonoBehaviour
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
    private float _damage;
    private float _attackRate;

    public void Setup(Tower tower)
    {
        _tower = tower;
        _towerImage.sprite = tower.towerRenderer.sprite;
        _towerImage.color = tower.towerColor.color;
        _nameText.text = tower.towerName;
        // �Ҽ��� ù° �ڸ����� �ݿø�
        _damage = tower.damage;
        // �Ҽ��� ��° �ڸ����� �ݿø�
        _attackRate = tower.attackRate;

        _damageText.text = Mathf.Round(_damage).ToString();
        _attackRateText.text = Math.Round(_attackRate, 2).ToString();
    }

    private void Update()
    {
        if(_damage != _tower.damage)
        {
            _damage = _tower.damage;
            _damageText.text = Mathf.Round(_damage).ToString();
        }

        if(_attackRate != _tower.attackRate)
        {
            _attackRate = _tower.attackRate;
            _attackRateText.text = Math.Round(_attackRate, 2).ToString();
        }
    }
}
