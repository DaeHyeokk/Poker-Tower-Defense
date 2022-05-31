using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 UIManager 오브젝트를 찾아 할당
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    [Header("Fade Text UI")]
    [SerializeField]
    private GameObject _systemMessagePrefab;
    private ObjectPool<SystemMessage> _systemMessagePool;
    [SerializeField]
    private GameObject _damageTakenTextPrefab;
    private ObjectPool<DamageTakenText> _damageTakenTextPool;

    [Header("Show Game Goods Infomation")]
    [SerializeField]
    private TextMeshProUGUI _cardChangeAmountText;
    [SerializeField]
    private TextMeshProUGUI _goldAmountText;
    [SerializeField]
    private TextMeshProUGUI _lifeAmountText;
    [SerializeField]
    private TextMeshProUGUI _mineralAmountText;

    [Header("Used by Card Drawer")]
    [SerializeField]
    private Image[] _cardImages;
    [SerializeField]
    private Image _towerPreviewImage;
    [SerializeField]
    private Button[] _changeButtons;
    [SerializeField]
    private TextMeshProUGUI _handText;
    [SerializeField]
    private TextMeshProUGUI _mineralGetText;
    [SerializeField]
    private Button _towerGambleButton;
    [SerializeField]
    private Button _mineralGambleButton;
    [SerializeField]
    private Button _getButton;
    [SerializeField]
    private Sprite _cardBackSprite;
    // Index로 원하는 스프라이트에 접근하기 때문에 배열에 등록하는 순서 중요!
    [SerializeField]
    private Sprite[] _cardSprites;
    [SerializeField]
    private Sprite[] _towerSprites;

    [Header("Used by Enemy Spawner")]
    [SerializeField]
    private Button[] _missionBossButtons;
    [SerializeField]
    private Slider[] _missionBossCooltimeSliders;
    [SerializeField]
    private TextMeshProUGUI[] _missionBossCooltimeTexts;

    [Header("Used by Tower")]
    [SerializeField]
    private Button[] _colorUpgradeButton;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCostTexts;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCountTexts;

    public ObjectPool<SystemMessage> systemMessagePool => _systemMessagePool;
    public ObjectPool<DamageTakenText> damageTakenTextPool => _damageTakenTextPool;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // 자신을 파괴
            return;
        }

        _systemMessagePool = new(_systemMessagePrefab, 5);
        _damageTakenTextPool = new(_damageTakenTextPrefab, 10);
    }

    public void ShowSystemMessage(string message)
    {
        SystemMessage systemMessage = _systemMessagePool.GetObject();

        systemMessage.transform.position = Vector3.zero;
        systemMessage.messageText.text = message;
    }

    public void ShowDamageTakenText(float damage, Transform target, DamageTakenType damageTakenType)
    {
        DamageTakenText damageTakenText = _damageTakenTextPool.GetObject();

        damageTakenText.transform.position = target.position;
        damageTakenText.damageTakenText.text = damage.ToString();
        damageTakenText.damageTakenType = damageTakenType;
    }

    public void SetCardChangeAmountText(int amount) => _cardChangeAmountText.text = amount.ToString();
    public void SetGoldAmountText(int amount) => _goldAmountText.text = amount.ToString();
    public void SetLiftAmountText(int amount) => _lifeAmountText.text = amount.ToString();
    public void SetMineralAmountText(int amount) => _mineralAmountText.text = amount.ToString();

    public void EnableUpgradeButton(int index) => _colorUpgradeButton[index].interactable = true;
    public void DisableUpgradeButton(int index) => _colorUpgradeButton[index].interactable = false;

    public void SetColorUpgradeCostText(int index, int amount) => _colorUpgradeCostTexts[index].text = amount.ToString() + 'M';
    public void SetColorUpgradeCountText(int index, int amount) => _colorUpgradeCountTexts[index].text = '+' + amount.ToString();

    public void ReverseCardFront(int index, Card card) => _cardImages[index].sprite = _cardSprites[card.index];
    public void ReverseCardBack(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void ShowChangeButton(int index) => _changeButtons[index].gameObject.SetActive(true);
    public void HideChangeButton(int index) => _changeButtons[index].gameObject.SetActive(false);

    public void ShowHandText() => _handText.gameObject.SetActive(true);
    public void HideHandText() => _handText.gameObject.SetActive(false);
    public void SetHandText(string handString) => _handText.text = handString;

    public void ShowMineralGetText() => _mineralGetText.gameObject.SetActive(true);
    public void HideMineralGetText() => _mineralGetText.gameObject.SetActive(false);
    public void SetMineralGetText(int amount) => _mineralGetText.text = '+' + amount.ToString() + 'M';

    public void SetTowerPreviewImage(int index) => _towerPreviewImage.sprite = _towerSprites[index];
    public void HideTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(false);
    public void ShowTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(true);

    public void HideTowerGambleButton() => _towerGambleButton.gameObject.SetActive(false);
    public void ShowTowerGambleButton() => _towerGambleButton.gameObject.SetActive(true);

    public void HideMineralGambleButton() => _mineralGambleButton.gameObject.SetActive(false);
    public void ShowMineralGambleButton() => _mineralGambleButton.gameObject.SetActive(true);

    public void HideGetButton() => _getButton.gameObject.SetActive(false);
    public void ShowGetButton() => _getButton.gameObject.SetActive(true);

    public void EnableMissionButton(int index) => _missionBossButtons[index].interactable = true;
    public void DisableMissionButton(int index) => _missionBossButtons[index].interactable = false;
    public void HideMissionBossCooltimeSlider(int index) => _missionBossCooltimeSliders[index].gameObject.SetActive(false);
    public void ShowMissionBossCooltimeSlider(int index) => _missionBossCooltimeSliders[index].gameObject.SetActive(true);
    public void SetMissionBossCooltimeSlider(int index, float maxValue)
    {
        _missionBossCooltimeSliders[index].maxValue = maxValue;
        _missionBossCooltimeSliders[index].value = maxValue;
        _missionBossCooltimeSliders[index].gameObject.SetActive(false);
    }
    public void ResetMissionBossCooltimeSliderValue(int index) => _missionBossCooltimeSliders[index].value = _missionBossCooltimeSliders[index].maxValue;
    public void DecreaseMissionBossCooltimeSliderValue(int index, float amount) => _missionBossCooltimeSliders[index].value -= amount; 
    public void SetMissionBossCooltimeText(int index, float cooltime) => _missionBossCooltimeTexts[index].text = cooltime.ToString();
}


/*
 * File : UIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * 게임에서 사용되는 모든 UI의 변경을 담당한다.
 */