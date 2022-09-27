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
    private Button[] _jokerCardButtons;
    [SerializeField]
    private Button[] _cardChangeButtons;
    [SerializeField]
    private Toggle _functionToggle;
    [SerializeField]
    private Image _functionToggleImage;
    [SerializeField]
    private Sprite _cardChangeSprite;
    [SerializeField]
    private TextMeshProUGUI _handText;
    [SerializeField]
    private Sprite _cardBackSprite;
    // Index로 원하는 스프라이트에 접근하기 때문에 배열에 등록하는 순서 중요!
    [SerializeField]
    private Sprite[] _cardSprites;
    [SerializeField]
    private Sprite _colorJokerSprite;
    [SerializeField]
    private Sprite _monochromeJokerSprite;

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

    [SerializeField]
    private GameObject _jokerCardUsingInfoPanelObject;

    public void ReverseCardFrountUI(int index)
    {
        int cardIndex = _cardGambler.drawCards[index].index;

        // 보여줄 카드가 조커카드가 아닐 경우 카드 바꾸기 버튼을 활성화 하고, 조커 카드 버튼을 비활성화 한다.
        if (cardIndex != Card.COLOR_JOKER_INDEX && cardIndex != Card.MONOCHROME_JOKER_INDEX)
        {
            ShowCardChangeButton(index);
            _cardImages[index].sprite = _cardSprites[_cardGambler.drawCards[index].index];
            _jokerCardButtons[index].interactable = false;
        }
        // 보여줄 카드가 조커카드일 경우 카드 바꾸기 버튼을 비활성화 하고, 조커 카드 버튼을 활성화 한다.
        // 조커 카드를 게임 설치 이후 처음 뽑았을 경우 조커 카드 사용법을 알려주는 오브젝트를 활성화 한다.
        else
        {
            if (cardIndex == Card.COLOR_JOKER_INDEX)
            {
                _cardImages[index].sprite = _colorJokerSprite;
                _jokerCardButtons[index].onClick.RemoveAllListeners();
                _jokerCardButtons[index].onClick.AddListener(() => _cardGambler.ShowCardSelector(index, CardSelector.JokerType.Color));
                _jokerCardButtons[index].interactable = true;
            }
            else
            {
                _cardImages[index].sprite = _monochromeJokerSprite;
                _jokerCardButtons[index].onClick.RemoveAllListeners();
                _jokerCardButtons[index].onClick.AddListener(() => _cardGambler.ShowCardSelector(index, CardSelector.JokerType.Monochrome));
                _jokerCardButtons[index].interactable = true;
            }

            HideCardChangeButton(index);

            if (!_cardGambler.isJokerCardPicked)
                ShowJokerCardUsingInfoPanel();
        }
    }

    public void AllReverseCardFrontUI()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
            ReverseCardFrountUI(index);
    }

    public void ReverseCardBackUI(int index) => _cardImages[index].sprite = _cardBackSprite;

    public void AllReverseCardBackUI()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
        {
            ReverseCardBackUI(index);
            HideCardChangeButton(index);
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

    public void ShowCardChangeButton(int index) => _cardChangeButtons[index].gameObject.SetActive(true);
    public void HideCardChangeButton(int index) => _cardChangeButtons[index].gameObject.SetActive(false);

    public void EnableCardChangeButton()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
            _cardChangeButtons[index].interactable = true;
    }

    public void DisableCardChangeButton()
    {
        for (int index = 0; index < _cardGambler.drawCardCount; index++)
            _cardChangeButtons[index].interactable = false;
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

        // 타워를 Get 하는 버튼을 비활성화 한다.
        HideGetButtonUI();

        // Gamble 버튼을 화면에 나타낸다.
        ShowGambleButtonUI();
    }

    public void MarkChangeCard(int changeIndex) => _cardImages[changeIndex].color = Color.yellow;
    public void MarkCancelChangeCard(int changeIndex) => _cardImages[changeIndex].color = Color.white;
    public void SetCardPattern(CardSelector.JokerType jokerType)
    {
        if(jokerType == CardSelector.JokerType.Color)
        {
            _patternButtons[(int)Card.Pattern.Clover].gameObject.SetActive(false);
            _patternButtons[(int)Card.Pattern.Diamond].gameObject.SetActive(true);
            _patternButtons[(int)Card.Pattern.Heart].gameObject.SetActive(true);
            _patternButtons[(int)Card.Pattern.Spade].gameObject.SetActive(false);
        }
        else
        {
            _patternButtons[(int)Card.Pattern.Clover].gameObject.SetActive(true);
            _patternButtons[(int)Card.Pattern.Diamond].gameObject.SetActive(false);
            _patternButtons[(int)Card.Pattern.Heart].gameObject.SetActive(false);
            _patternButtons[(int)Card.Pattern.Spade].gameObject.SetActive(true);
        }
    }
    public void ChangeSelectCardPattern(Card.Pattern oldPattern, Card.Pattern newPattern)
    {
        _patternButtons[(int)oldPattern].interactable = true;
        _patternButtons[(int)newPattern].interactable = false;

        ChangeCardButtonImage(newPattern);
        DisableCardButton(newPattern);
    }

    private void ChangeCardButtonImage(Card.Pattern newPattern)
    {
        // 카드 전체 이미지를 다른 무늬로 바꾸고 활성화 한다.
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
        // 이미 뽑은 카드들이 있다면 비활성화 한다.
        for (int i = 0; i < _cardGambler.drawCards.Length; i++)
        {
            // 뽑은 카드가 조커카드라면 건너뛴다.
            if (_cardGambler.drawCards[i].isJoker)
                continue;

            if ((int)_cardGambler.drawCards[i].pattern == (int)newPattern)
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
    
    public void ShowJokerCardUsingInfoPanel()
    {
        _cardGambler.isJokerCardPicked = true;
        _jokerCardUsingInfoPanelObject.SetActive(true);
    }
}


/*
 * File : GambleUIController.cs
 * First Update : 2022/04/27 WED 11:10
 * 인게임에서 카드와 관련된 UI를 제어하는 스크립트.
 * 전체 UI의 변경은 StageUIManager에서 수행하기 때문에
 * StageUIManager에 정의된 메서드들을 호출하여 UI를 제어한다.
 * 
 * Update : 2022/06/02 THU 20:10
 * StageUIManager에 정의된 메서드를 호출하여 UI를 제어하는 방식이 아닌 직접 UI 요소를 참조해서 제어하는 방식으로 변경.
 */