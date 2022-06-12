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
        _damage = tower.damage;
        _attackRate = tower.attackRate;

        // 소수점 첫째 자리에서 반올림
        _damageText.text = Mathf.Round(_damage).ToString();
        // 소수점 둘째 자리에서 반올림
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
