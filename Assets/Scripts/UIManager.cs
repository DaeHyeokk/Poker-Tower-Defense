using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 ObjectPool 오브젝트를 찾아 할당
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }
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
    
    [Header("Used by Tower")]
    [SerializeField]
    private Button[] _colorUpgradeButton;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCostTexts;
    [SerializeField]
    private TextMeshProUGUI[] _colorUpgradeCountTexts;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);    // 자신을 파괴
            return;
        }
    }

    public void EnableMissionButton(int index) => _missionBossButtons[index].interactable = true;
    public void DisableMissionButton(int index) => _missionBossButtons[index].interactable = false;

    public void EnableUpgradeButton(int index) => _colorUpgradeButton[index].interactable = true;
    public void DisableUpgradeButton(int index) => _colorUpgradeButton[index].interactable = false;
    public void SetColorUpgradeCostText(int index, int amount) => _colorUpgradeCostTexts[index].text = amount.ToString() + 'M';
    public void SetColorUpgradeCountText(int index, int amount) => _colorUpgradeCountTexts[index].text = '+' + amount.ToString();

    public void ReverseCardFront(int index, Card card) => _cardImages[index].sprite = _cardSprites[card.index];
    public void ReverseCardBack(int index) => _cardImages[index].sprite = _cardBackSprite;
    public void ShowChangeButton(int index) => _changeButtons[index].gameObject.SetActive(true);
    public void HideChangeButton(int index) => _changeButtons[index].gameObject.SetActive(false);
    public void HideHandText() => _handText.gameObject.SetActive(false);
    public void ShowHandText() => _handText.gameObject.SetActive(true);
    public void SetHandText(string handString) => _handText.text = handString;
    public void SetTowerPreviewImage(int index) => _towerPreviewImage.sprite = _towerSprites[index];
    public void HideTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(false);
    public void ShowTowerPreviewImage() => _towerPreviewImage.gameObject.SetActive(true);
    public void HideTowerGambleButton() => _towerGambleButton.gameObject.SetActive(false);
    public void ShowTowerGambleButton() => _towerGambleButton.gameObject.SetActive(true);
    public void HideMineralGambleButton() => _mineralGambleButton.gameObject.SetActive(false);
    public void ShowMineralGambleButton() => _mineralGambleButton.gameObject.SetActive(true);
    public void HideGetButton() => _getButton.gameObject.SetActive(false);
    public void ShowGetButton() => _getButton.gameObject.SetActive(true);

    public void SetCardChangeAmountText(int amount) => _cardChangeAmountText.text = amount.ToString();
    public void SetGoldAmountText(int amount) => _goldAmountText.text = amount.ToString();
    public void SetLiftAmountText(int amount) => _lifeAmountText.text = amount.ToString();
    public void SetMineralAmountText(int amount) => _mineralAmountText.text = amount.ToString();
}


/*
 * File : UIManager.cs
 * First Update : 2022/04/27 WED 23:10
 * 게임에서 사용되는 모든 UI의 변경을 담당한다.
 */