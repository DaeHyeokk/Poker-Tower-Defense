using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GambleUIController : MonoBehaviour
{
    [SerializeField]
    private CardGambler _cardGambler;
    [SerializeField]
    private Image[] _cardImages;
    [SerializeField]
    private Button[] _functionButtons;
    [SerializeField]
    private Image[] _functionButtonImages;
    [SerializeField]
    private Button _functionToggleButton;
    [SerializeField]
    private Image _functionToggleImage;
    [SerializeField]
    private Sprite _cardChangeSprite;
    [SerializeField]
    private Sprite _jokerCardSprite;
    [SerializeField]
    private TextMeshProUGUI _handText;
    [SerializeField]
    private Sprite _cardBackSprite;
    // Index로 원하는 스프라이트에 접근하기 때문에 배열에 등록하는 순서 중요!
    [SerializeField]
    private Sprite[] _cardSprites;

    [Header("Gamble Button UI Canvas")]
    [SerializeField]
    private Button _towerGambleButton;
    [SerializeField]
    private Button _mineralGambleButton;
    [SerializeField]
    private Button _getButton;
    [SerializeField]
    private Image _towerPreviewImage;
    [SerializeField]
    private Sprite[] _towerSprites;
    [SerializeField]
    private TextMeshProUGUI _mineralGetText;

    public int drawCardCount { get; set; }

    public void ReverseCardFrountUI(int index)
    {
        _cardImages[index].sprite = _cardSprites[_cardGambler.drawCards[index].index];
        ShowFunctionButton(index);
    }

    public void AllReverseCardFrontUI()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
            ReverseCardFrountUI(index);
    }

    public void ReverseCardBackUI(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void AllReverseCardBackUI()
    {
        for (int index = 0; index < drawCardCount; index++)
        {
            ReverseCardBackUI(index);
            HideFunctionButton(index);
        }
    }

    public void ShowResultUI()
    {
        SetHandUI(_cardGambler.drawHand);

        if (_cardGambler.gambleType == CardGambler.GambleType.Tower)
            SetTowerPreviewUI((int)_cardGambler.drawHand);
        else
            SetMineralPreviewUI(_cardGambler.mineralGambleAmounts[(int)_cardGambler.drawHand]);

        ShowGetButtonUI();
    }

    public void ShowFunctionButton(int index) => _functionButtons[index].gameObject.SetActive(true);
    public void HideFunctionButton(int index) => _functionButtons[index].gameObject.SetActive(false);

    public void ShowFunctionToggleButton() => _functionToggleButton.gameObject.SetActive(true);
    public void HideFunctionToggleButton() => _functionToggleButton.gameObject.SetActive(false);

    public void ToggleFunctionImage()
    {
        if (_cardGambler.buttonFunctionType == CardGambler.ButtonFunctionType.CardChange)
            _functionToggleImage.sprite = _jokerCardSprite;
        else
            _functionToggleImage.sprite = _cardChangeSprite;

        ToggleFunctionButtonImage();
    }
    private void ToggleFunctionButtonImage()
    {
        for (int i = 0; i < _functionButtons.Length; i++)
        {
            if (_cardGambler.buttonFunctionType == CardGambler.ButtonFunctionType.CardChange)
                _functionButtonImages[i].sprite = _cardChangeSprite;
            else
                _functionButtonImages[i].sprite = _jokerCardSprite;
        }
    }

    public void SetHandUI(PokerHand drawHand)
    {
        _handText.text = drawHand.ToString();
        _handText.gameObject.SetActive(true);
    }

    public void ShowGambleButtonUI()
    {
        _towerGambleButton.gameObject.SetActive(true);
        _mineralGambleButton.gameObject.SetActive(true);
    }

    public void HideGambleButtonUI()
    {
        _towerGambleButton.gameObject.SetActive(false);
        _mineralGambleButton.gameObject.SetActive(false);
    }

    public void EnableGambleButtonUI()
    {
        _towerGambleButton.interactable = true;
        _mineralGambleButton.interactable = true; ;

    }

    public void DisableGambleButtonUI()
    {
        _towerGambleButton.interactable = false;
        _mineralGambleButton.interactable = false;
    }

    public void SetTowerPreviewUI(int towerIndex)
    {
        _towerPreviewImage.sprite = _towerSprites[towerIndex];
        _towerPreviewImage.gameObject.SetActive(true);
    }

    public void SetMineralPreviewUI(int mineralAmount)
    {
        _mineralGetText.text = '+' + mineralAmount.ToString() + 'M';
        _mineralGetText.gameObject.SetActive(true);
    }

    public void ShowGetButtonUI() => _getButton.gameObject.SetActive(true);
    public void HideGetButtonUI()
    {
        _getButton.gameObject.SetActive(false);
        _towerPreviewImage.gameObject.SetActive(false);
        _mineralGetText.gameObject.SetActive(false);
    }

    public void ResetGambleUI()
    {
        // 만들어진 족보를 나타내는 텍스트를 화면에서 숨긴다.
        _handText.gameObject.SetActive(false);

        // 카드를 모두 뒤집는다.
        AllReverseCardBackUI();
        HideFunctionToggleButton();
        // 타워를 Get 하는 버튼을 화면에서 숨긴다.
        HideGetButtonUI();
        // Gamble 버튼을 화면에 나타낸다.
        ShowGambleButtonUI();
    }
}


/*
 * File : GambleUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * 인게임에서 카드와 관련된 UI를 제어하는 스크립트.
 * 전체 UI의 변경은 UIManager에서 수행하기 때문에
 * UIManager에 정의된 메서드들을 호출하여 UI를 제어한다.
 * 
 * Update : 2022/06/02 THU 20:10
 * UIManager에 정의된 메서드를 호출하여 UI를 제어하는 방식이 아닌 직접 UI 요소를 참조해서 제어하는 방식으로 변경.
 */