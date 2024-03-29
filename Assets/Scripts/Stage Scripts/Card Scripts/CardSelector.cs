using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, None }
    public enum JokerType { Color, Monochrome }

    [SerializeField]
    private GambleUIController _gambleUIController;
    [SerializeField]
    private CardGambler _cardGambler;

    private Card.Pattern _patternIndex;
    private Number _numberIndex;
    private JokerType _jokerType;

    public Card.Pattern patternIndex
    {
        get => _patternIndex;
        set
        {
            _gambleUIController.ChangeSelectCardPattern(_patternIndex, value);
            _patternIndex = value;
            numberIndex = Number.None;
        }
    }
    
    public Number numberIndex
    {
        get => _numberIndex;
        set
        {
            _gambleUIController.ChangeSelectCardButton(_numberIndex, value);
            _numberIndex = value;
        }
    }
    
    public JokerType jokerType
    {
        get => _jokerType;
        set
        {
            _jokerType = value;

            if (value == JokerType.Color)
            {
                patternIndex = Card.Pattern.Diamond;
                _gambleUIController.SetCardPattern(JokerType.Color);
            }
            else
            {
                patternIndex = Card.Pattern.Clover;
                _gambleUIController.SetCardPattern(JokerType.Monochrome);
            }
        }
    }

    public void OnClickChangePattern(int patternIndex)
    {
        this.patternIndex = (Card.Pattern)patternIndex;
    }
    public void OnClickSelectionNumber(int numberIndex) 
    {
        // 이미 선택된 카드를 다시 클릭하는 경우 선택을 취소한다.
        if (this.numberIndex == (Number)numberIndex)
            this.numberIndex = Number.None;
        else
            this.numberIndex = (Number)numberIndex; 
    }
    public void OnClickChangeCard()
    {
        int cardIndex = (int)numberIndex + ((int)patternIndex * Card.MAX_NUMBER);
        _cardGambler.CardSelectChange(cardIndex);
        _cardGambler.HideCardSelector();
    }
    public void OnClickCloseCardSelector() => _cardGambler.HideCardSelector();
}
