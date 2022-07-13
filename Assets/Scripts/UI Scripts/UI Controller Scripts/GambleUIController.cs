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
    // Index�� ���ϴ� ��������Ʈ�� �����ϱ� ������ �迭�� ����ϴ� ���� �߿�!
    [SerializeField]
    private Sprite[] _cardSprites;

    [Header("Gamble Button UI")]
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

    [Header("Card Selector UI")]
    [SerializeField]
    private Button[] _patternButtons;
    [SerializeField]
    private Button[] _cardButtons;
    [SerializeField]
    private Image[] _cardBtnImages;
    [SerializeField]
    private Button _changeButton;

    public void ReverseCardFrountUI(int index) => _cardImages[index].sprite = _cardSprites[_cardGambler.drawCards[index].index];

    public void AllReverseCardFrontUI()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
        {
            ReverseCardFrountUI(index);
            ShowFunctionButton(index);
        }
    }

    public void ReverseCardBackUI(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void AllReverseCardBackUI()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
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

    public void EnableFunctionToggleButton() => _functionToggleButton.interactable = true;
    public void DisableFunctionToggleButton() => _functionToggleButton.interactable = false;

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
        // ������� ������ ��Ÿ���� �ؽ�Ʈ�� ȭ�鿡�� �����.
        _handText.gameObject.SetActive(false);

        // ī�带 ��� �����´�.
        AllReverseCardBackUI();
        // ī�� �ٲٱ� ��ɰ� ��Ŀī�� ��� ��ư�� ��Ȱ��ȭ �Ѵ�.
        HideFunctionToggleButton();
        // Ÿ���� Get �ϴ� ��ư�� ��Ȱ��ȭ �Ѵ�.
        HideGetButtonUI();
        // Gamble ��ư�� ȭ�鿡 ��Ÿ����.
        ShowGambleButtonUI();
    }

    public void MarkChangeCard(int changeIndex) => _cardImages[changeIndex].color = Color.yellow;
    public void MarkCancelChangeCard(int changeIndex) => _cardImages[changeIndex].color = Color.white;

    public void ChangeSelectCardPattern(Card.Pattern oldPattern, Card.Pattern newPattern)
    {
        _patternButtons[(int)oldPattern].interactable = true;
        _patternButtons[(int)newPattern].interactable = false;

        ChangeCardButtonImage(newPattern);
        DisableCardButton(newPattern);
    }

    private void ChangeCardButtonImage(Card.Pattern newPattern)
    {
        // ī�� ��ü �̹����� �ٸ� ���̷� �ٲٰ� Ȱ��ȭ �Ѵ�.
        for (int i = 0; i < _cardBtnImages.Length; i++)
        {
            int cardIndex = i + ((int)newPattern * Card.MAX_NUMBER);
            _cardButtons[i].interactable = true;
            _cardBtnImages[i].sprite = _cardSprites[cardIndex];
            _cardBtnImages[i].color = Color.white;
        }
    }
    
    private void DisableCardButton(Card.Pattern newPattern)
    {
        // �̹� ���� ī����� �ִٸ� ��Ȱ��ȭ �Ѵ�.
        for(int i=0; i<_cardGambler.drawCards.Length; i++)
        {
            if((int)_cardGambler.drawCards[i].pattern == (int)newPattern)
            {
                int numberIndex = (int)_cardGambler.drawCards[i].number;
                _cardButtons[numberIndex].interactable = false;
            }
        }
    }

    public void ChangeSelectCardButton(CardSelector.Number oldNumber, CardSelector.Number newNumber)
    {
        if(oldNumber != CardSelector.Number.None)
            _cardBtnImages[(int)oldNumber].color = Color.white;

        if (newNumber != CardSelector.Number.None)
        {
            _cardBtnImages[(int)newNumber].color = Color.yellow;
            _changeButton.interactable = true;
        }
        else
            _changeButton.interactable = false;
    }
    
}


/*
 * File : GambleUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * �ΰ��ӿ��� ī��� ���õ� UI�� �����ϴ� ��ũ��Ʈ.
 * ��ü UI�� ������ UIManager���� �����ϱ� ������
 * UIManager�� ���ǵ� �޼������ ȣ���Ͽ� UI�� �����Ѵ�.
 * 
 * Update : 2022/06/02 THU 20:10
 * UIManager�� ���ǵ� �޼��带 ȣ���Ͽ� UI�� �����ϴ� ����� �ƴ� ���� UI ��Ҹ� �����ؼ� �����ϴ� ������� ����.
 */