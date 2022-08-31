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
    private Tower _tower;
    private float _damage;
    private float _attackRate;
    private float _upgradeDIP;
    private float _upgradeRIP;
    private bool _isLocking;
    private bool _isHiding;

    public PopupUIAnimation popupUIAnimation => _popupUIAnimation;

    private void Awake()
    {
        _popupUIAnimation = GetComponent<PopupUIAnimation>();
        // �˾� �ִϸ��̼� �� ���� �۾����� �޼ҵ尡 �Ϸ�� �� ������ �۾� ����. (������Ʈ ��Ȱ��ȭ)
        //_popupUIAnimation.onCompletionSmaller += () => StageUIManager.instance.HideTowerDetailInfo();
        _popupUIAnimation.onCompletionSmaller += () => this.gameObject.SetActive(false);

        _hideTimerSlider.maxValue = _hideDelay;
    }

    private void OnEnable()
    {
        _popupUIAnimation.StartBiggerAnimation();

        SoundManager.instance.PlaySFX(SoundFileNameDictionary.towerDetailInfoUIShowSound);

        _hideTimerSlider.value = _hideTimerSlider.maxValue;
        _isHiding = false;
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

        // Ÿ�̸Ӱ� ������ �ڵ����� ������� ����.
        // UI�� ����� ���°ų� ������� ���̶�� �ǳʶڴ�.
        if (_isLocking || _isHiding)
            return;
        else
        {
            _hideTimerSlider.value -= Time.unscaledDeltaTime;

            if (_hideTimerSlider.value <= 0f)
            {
                _isHiding = true;
                HideObject();
            }
        }
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

        _damageText.text = Mathf.Round(_damage).ToString();
        _attackRateText.text = Math.Round(_attackRate, 2).ToString();
        _upgradeDIPText.text = _upgradeDIP.ToString();
        _upgradeRIPText.text = _upgradeRIP.ToString();

        _baseAttackDetailText.text = tower.detailBaseAttackInfo.ToString();
        _specialAttackDetailText.text = tower.detailSpecialAttackInfo.ToString();
    }

    public void ResetHideDelay()
    {
        _hideTimerSlider.value = _hideTimerSlider.maxValue;
    }

    public void HideObject()
    {
        _popupUIAnimation.StartSmallerAnimation();

        SoundManager.instance.PlaySFX(SoundFileNameDictionary.towerDetailInfoUIHideSound);
    }

    public void OnClickLockToggle(bool value)
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
