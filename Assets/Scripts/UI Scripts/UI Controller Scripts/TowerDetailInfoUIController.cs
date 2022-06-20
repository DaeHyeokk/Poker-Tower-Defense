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
    private Image _lockImage;
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
    private Slider _hideTimerSlider;
    [SerializeField]
    private float _hideDelay;

    private PopupUIAnimation _popupUIAnimation;
    private WaitForSecondsRealtime _realPointZeroFiveSecond = new(0.05f);
    private Tower _tower;
    private float _damage;
    private float _attackRate;
    private float _upgradeDIP;
    private float _upgradeRIP;
    private float _remainHideDelay;
    private bool _isLocking;

    private void Awake()
    {
        _popupUIAnimation = GetComponent<PopupUIAnimation>();
        // �˾� �ִϸ��̼� �� ���� �۾����� �޼ҵ尡 �Ϸ�� �� ������ �۾� ����. (������Ʈ ��Ȱ��ȭ)
        _popupUIAnimation.onCompletionSmaller += () => this.gameObject.SetActive(false);
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
        _hideTimerSlider.maxValue = _hideDelay;
        _hideTimerSlider.value = _hideDelay;

        // �Ҽ��� ù��° �ڸ����� �ݿø�
        _damageText.text = _damage.ToString();
        // �Ҽ��� �ι�° �ڸ����� �ݿø�
        _attackRateText.text = _attackRate.ToString();
        // �Ҽ��� ù��° �ڸ����� �ݿø�
        _upgradeDIPText.text = _upgradeDIP.ToString();
        // �Ҽ��� �׹�° �ڸ����� �ݿø�
        _upgradeRIPText.text = _upgradeRIP.ToString();

        _baseAttackDetailText.text = tower.detailBaseAttackInfo.ToString();
        _specialAttackDetailText.text = tower.detailSpecialAttackInfo.ToString();
    }

    private void OnEnable()
    {
        _popupUIAnimation.StartBiggerAnimation();
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
            yield return _realPointZeroFiveSecond;
            if (!_isLocking)
            {
                _remainHideDelay -= 0.05f;
                _hideTimerSlider.value -= 0.05f;
            }
        }

        HideObject();
    }

    public void ResetHideDelay()
    {

        _remainHideDelay = _hideDelay;
        _hideTimerSlider.value = _hideDelay;
    }

    public void HideObject()
    {
        _popupUIAnimation.StartSmallerAnimation();
    }

    public void ToggleLockButton()
    {
        if(_isLocking)
        {
            ResetHideDelay();
            _isLocking = false;
            _lockImage.sprite = _unlockSprite;
        }
        else
        {
            _hideTimerSlider.value = 0f;
            _isLocking = true;
            _lockImage.sprite = _lockSprite;
        }
    }
}
