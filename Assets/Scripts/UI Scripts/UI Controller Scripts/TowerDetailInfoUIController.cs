using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDetailInfoUIController : MonoBehaviour
{
    [SerializeField]
    private Image _towerImage;
    [SerializeField]
    private Image _lockButtonImage;
    [SerializeField]
    private Sprite _lockSprite;
    [SerializeField]
    private Sprite _unlockSprite;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _damageText;
    [SerializeField]
    private TextMeshProUGUI _upgradeDIPText;
    [SerializeField]
    private TextMeshProUGUI _attackRateText;
    [SerializeField]
    private TextMeshProUGUI _upgradeRIPText;
    [SerializeField]
    private TextMeshProUGUI _baseAttackDetailText;
    [SerializeField]
    private TextMeshProUGUI _specialAttackDetailText;
    [SerializeField]
    private float _hideDelay;

    private WaitForSeconds _oneSecond;
    private Tower _tower;
    private float _damage;
    private float _attackRate;
    private float _upgradeDIP;
    private float _upgradeRIP;
    private float _remainHideDelay;
    private bool _isLocking;

    private void Awake()
    {
        _oneSecond = new(1f);
    }

    public void Setup(Tower tower)
    {
        _tower = tower;
        _towerImage.sprite = tower.towerRenderer.sprite;
        _towerImage.color = tower.towerColor.color;
        _nameText.text = tower.towerName;
        _damage = tower.damage;
        _attackRate = tower.attackRate;
        _upgradeDIP = tower.upgradeDIP;
        _upgradeRIP = tower.upgradeRIP;
        _remainHideDelay = _hideDelay;
        
        // �Ҽ��� ù��° �ڸ����� �ݿø�
        _damageText.text = Mathf.Round(_damage).ToString();
        // �Ҽ��� �ι�° �ڸ����� �ݿø�
        _attackRateText.text = Math.Round(_attackRate, 2).ToString();
        // �Ҽ��� ù��° �ڸ����� �ݿø�
        _upgradeDIPText.text = Mathf.Round(_upgradeDIP).ToString();
        // �Ҽ��� �׹�° �ڸ����� �ݿø�
        _upgradeRIPText.text = Math.Round(_upgradeRIP, 4).ToString();

        _baseAttackDetailText.text = tower.detailBaseAttackInfo.ToString();
        _specialAttackDetailText.text = tower.detailSpecialAttackInfo.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(AutoHideUICoroutine());
    }

    private void Update()
    {
        if (_damage != _tower.damage)
        {
            _damage = _tower.damage;
            _damageText.text = Mathf.Round(_damage).ToString();
        }

        if (_attackRate != _tower.attackRate)
        {
            _attackRate = _tower.attackRate;
            _attackRateText.text = Math.Round(_attackRate, 2).ToString();
        }
    }

    // �ð��� ������ �ڵ����� ������� �ڷ�ƾ �޼ҵ�
    private IEnumerator AutoHideUICoroutine()
    {
        while(_remainHideDelay > 0)
        {
            yield return _oneSecond;
            if (!_isLocking) _remainHideDelay--;
        }

        HideObject();
    }

    public void ResetHideDelay() => _remainHideDelay = _hideDelay;

    public void HideObject()
    {
        this.gameObject.SetActive(false);
    }

    public void ToggleLockButton()
    {
        if(_isLocking)
        {
            _isLocking = false;
            _lockButtonImage.sprite = _unlockSprite;
        }
        else
        {
            _isLocking = true;
            _lockButtonImage.sprite = _lockSprite;
        }
    }
}
