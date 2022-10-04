using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGambler : MonoBehaviour
{
    public enum GambleType { Tower, Mineral }

    [SerializeField]
    private GambleUIController _gambleUIController;
    [SerializeField]
    private CardSelector _cardSelector;
    [SerializeField]
    private int[] _mineralGambleAmounts;

    private TowerBuilder _towerBuilder;
    private CardDrawer _cardDrawer;
    // 무엇을 위해(타워짓기, 미네랄뽑기) 카드를 뽑는지를 나타내는 변수
    private GambleType _gambleType;

    private int _changeIndex;
    private bool _isGambled;
    private bool _isJokerCardPicked;

    public int drawCardCount => _cardDrawer.drawCards.Length;
    public Card[] drawCards => _cardDrawer.drawCards;
    public PokerHand drawHand => _cardDrawer.drawHand;
    public GambleType gambleType => _gambleType;
    public int[] mineralGambleAmounts => _mineralGambleAmounts;

    public bool isJokerCardPicked
    {
        get => _isJokerCardPicked;
        set
        {
            _isJokerCardPicked = value;

            if (value)
                PlayerPrefs.SetString("IsJokerCardPicked", "True");
        }
    }

    private void Awake()
    {
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _cardDrawer = new CardDrawer();
        _isGambled = false;
        _isJokerCardPicked = (PlayerPrefs.GetString("IsJokerCardPicked") == "True");
    }

    public void StartGamble(int gambleType)
    {
        // 이미 Gamble을 진행하고 있는 중이라면 수행하지 않는다. (혹시모를 버그 방지)
        if (_isGambled) return;

        // Gamble을 진행하기 위한 100골드를 보유하고 있지 않다면 수행하지 않는다.
        if (StageManager.instance.gold < 100)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughGold);
            return;
        }

        // 플레이어의 골드에서 100골드를 차감한다.
        StageManager.instance.gold -= 100;
        // 카드 뽑는 사운드 재생.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);

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
        _isGambled = true;

        // 뽑기 버튼을 화면에서 숨긴다.
        _gambleUIController.HideGambleButtonUI();

        // 카드를 뽑는다.
        _cardDrawer.DrawCardAll();

        // 플레이어 화면에 새로 뽑은 카드를 보여준다.
        _gambleUIController.AllReverseCardFrontUI();

        // 뽑은 카드 결과값 갱신.
        _gambleUIController.ShowResultUI();
    }

    public void OnClickCardChangeButton(int changeIndex) => CardRandomChange(changeIndex);

    private void CardRandomChange(int changeIndex)
    {
        // 플레이어의 ChangeChance 횟수가 0 이하라면 수행하지 않는다.
        if (StageManager.instance.changeChance <= 0)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughChangeChance);
            return;
        }

        // 플레이어의 ChangeChance 횟수를 1 차감한다.
        StageManager.instance.changeChance--;
        // 카드 뽑는 사운드 재생.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);
        // 카드를 바꾼다.
        _cardDrawer.ChangeRandomCard(changeIndex);

        // 바꾼 카드를 플레이어에게 보여준다.
        _gambleUIController.ReverseCardFrountUI(changeIndex);
        // 뽑은 카드 결과값 갱신.
        _gambleUIController.ShowResultUI();
    }

    public void ShowCardSelector(int changeIndex, CardSelector.JokerType jokerType)
    {
        _cardSelector.jokerType = jokerType;

        // 이미 Card Selector가 활성화 되어있는 경우 바꿀 카드의 index만 바꾼다.
        if (_cardSelector.gameObject.activeSelf)
        {
            _gambleUIController.MarkCancelChangeCard(_changeIndex);

            _changeIndex = changeIndex;

            _gambleUIController.MarkChangeCard(_changeIndex);

        }
        else
        {
            _changeIndex = changeIndex;
            _gambleUIController.MarkChangeCard(_changeIndex);
            _gambleUIController.DisableCardChangeButton();
            _cardSelector.gameObject.SetActive(true);
        }
    }
    
    public void HideCardSelector()
    {
        _gambleUIController.MarkCancelChangeCard(_changeIndex);
        _gambleUIController.EnableCardChangeButton();
        _cardSelector.gameObject.SetActive(false);
    }

    public void CardSelectChange(int cardIndex)
    {
        // 카드 뽑는 사운드 재생.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);

        HideCardSelector();
        // 전달받은 카드 번호로 카드를 바꾼다.
        _cardDrawer.ChangeSelectCard(_changeIndex, cardIndex);
        // 바꾼 카드를 플레이어에게 보여준다.
        _gambleUIController.ReverseCardFrountUI(_changeIndex);
        // 뽑은 카드 결과값 갱신.
        _gambleUIController.ShowResultUI();
    }

    public void GetResult()
    {
        if (_gambleType == GambleType.Tower)
            _towerBuilder.BuildTower((int)_cardDrawer.drawHand);
        else
            StageManager.instance.mineral += _mineralGambleAmounts[(int)_cardDrawer.drawHand];

        ResetGambler();
    }

    public void ResetGambler()
    {
        _isGambled = false;
        _cardDrawer.ResetDrawer();
        _gambleUIController.ResetGambleUI();

        if (_cardSelector.gameObject.activeSelf)
            HideCardSelector();
    }
}
