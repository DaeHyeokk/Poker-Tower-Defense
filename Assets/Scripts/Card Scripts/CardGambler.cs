using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGambler : MonoBehaviour
{
    public enum GambleType { Tower, Mineral }
    public enum ButtonFunctionType { CardChange, JokerCard }

    [SerializeField]
    private GambleUIController _gambleUIController;
    [SerializeField]
    private int[] _mineralGambleAmounts;

    private CardDrawer _cardDrawer;
    private WaitForSeconds _waitPointOne = new(0.1f);
    private int _changeIndex;

    // 무엇을 위해(타워짓기, 미네랄뽑기) 카드를 뽑는지를 나타내는 변수
    private GambleType _gambleType;
    // 화면에서 카드 UI 밑의 버튼을 눌렀을 때 수행하는 기능을 나타내는 변수 (초기 설정값은 Card Change 기능)
    private ButtonFunctionType _buttonFunctionType = ButtonFunctionType.CardChange;
    private bool _isGambling;

    public int drawCardCount => _cardDrawer.drawCards.Length;
    public Card[] drawCards => _cardDrawer.drawCards;
    public PokerHand drawHand => _cardDrawer.drawHand;
    public GambleType gambleType => _gambleType;
    public ButtonFunctionType buttonFunctionType
    {
        get => _buttonFunctionType;
        set
        { 
            _buttonFunctionType = value;
            _gambleUIController.ToggleFunctionImage();
        }
    }
    public int[] mineralGambleAmounts => _mineralGambleAmounts;

    private void Awake()
    {
        _cardDrawer = new CardDrawer();
        _gambleUIController.drawCardCount = _cardDrawer.drawCards.Length;
        _isGambling = false;
    }

    public void StartGamble(int gambleType)
    {
        // 이미 Gamble을 진행하고 있는 중이라면 수행하지 않는다. (혹시모를 버그 방지)
        if (_isGambling) return;

        // Gamble을 진행하기 위한 100골드를 보유하고 있지 않다면 수행하지 않는다.
        if (GameManager.instance.gold < 100)
        {
            UIManager.instance.ShowSystemMessage("골드가 부족합니다.");
            return;
        }

        // 플레이어의 골드에서 100골드를 차감한다.
        GameManager.instance.gold -= 100;

        if (gambleType == (int)GambleType.Tower)
        {
            // Gambler의 gamble type을 Tower로 변경
            _gambleType = GambleType.Tower;
        }
        else
        {
            // Gambler의 gamble type을 Mineral로 변경
            _gambleType = GambleType.Mineral;
        }

        // Gamble을 진행 중인 상태로 바꾼다.
        _isGambling = true;

        // 뽑기 버튼을 화면에서 숨긴다.
        _gambleUIController.HideGambleButtonUI();

        // 카드를 뽑는다.
        _cardDrawer.DrawCardAll();

        // 플레이어 화면에 새로 뽑은 카드를 보여준다.
        _gambleUIController.AllReverseCardFrontUI();
        _gambleUIController.ShowFunctionToggleButton();
        // 뽑은 카드 결과값 갱신.
        _gambleUIController.ShowResultUI();
    }

    public void ToggleButtonFunction()
    {
        if (buttonFunctionType == ButtonFunctionType.CardChange)
            buttonFunctionType = ButtonFunctionType.JokerCard;
        else
            buttonFunctionType = ButtonFunctionType.CardChange;
    }

    public void ExecuteButtonFunction(int changeIndex)
    {
        if (_buttonFunctionType == ButtonFunctionType.CardChange)
            CardRandomChange(changeIndex);
        else
            UseJokerCard(changeIndex);
    }

    private void CardRandomChange(int changeIndex)
    {
        // 플레이어의 ChangeChance 횟수가 0 이하라면 수행하지 않는다.
        if (GameManager.instance.changeChance <= 0)
        {
            UIManager.instance.ShowSystemMessage("카드 교환권이 부족합니다.");
            return;
        }

        // 플레이어의 ChangeChance 횟수를 1 차감한다.
        GameManager.instance.changeChance--;

        _changeIndex = changeIndex;

        // 플레이어 화면에 오픈된 카드 중 바꿀 카드를 뒤집는다.
        _gambleUIController.ReverseCardBackUI(_changeIndex);

        // 카드를 바꾼다.
        _cardDrawer.ChangeRandomCard(_changeIndex);

        // 바꾼 카드를 플레이어에게 보여준다.
        _gambleUIController.ReverseCardFrountUI(changeIndex);
        // 뽑은 카드 결과값 갱신.
        _gambleUIController.ShowResultUI();
    }

    private void UseJokerCard(int cardIndex)
    {
        // 플레이어의 JokerCard 개수가 0개 이하라면 수행하지 않는다.
        if (GameManager.instance.jokerCard <= 0)
        {
            UIManager.instance.ShowSystemMessage("조커 카드가 부족합니다.");
            return;
        }

    }

    private void CardSelectChange(int changeIndex)
    {
        // 플레이어의 JokerCard 개수가 0개 이하라면 수행하지 않는다.
        if (GameManager.instance.jokerCard <= 0)
        {
            UIManager.instance.ShowSystemMessage("조커 카드가 부족합니다.");
            return;
        }
    }


    public void GetResult()
    {
        if (_gambleType == GambleType.Tower)
            TowerBuilder.instance.BuildTower((int)_cardDrawer.drawHand);
        else
            GameManager.instance.mineral += _mineralGambleAmounts[(int)_cardDrawer.drawHand];

        ResetGambler();
    }

    public void ResetGambler()
    {
        _isGambling = false;
        _cardDrawer.ResetDrawer();
        _gambleUIController.ResetGambleUI();
    }
}
