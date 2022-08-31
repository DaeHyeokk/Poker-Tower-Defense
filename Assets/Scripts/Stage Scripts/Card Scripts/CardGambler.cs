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
    private CardSelector _cardSelector;
    [SerializeField]
    private int[] _mineralGambleAmounts;

    private TowerBuilder _towerBuilder;
    private CardDrawer _cardDrawer;
    // ������ ����(Ÿ������, �̳׶��̱�) ī�带 �̴����� ��Ÿ���� ����
    private GambleType _gambleType;
    // ȭ�鿡�� ī�� UI ���� ��ư�� ������ �� �����ϴ� ����� ��Ÿ���� ���� (�ʱ� �������� Card Change ���)
    private ButtonFunctionType _buttonFunctionType = ButtonFunctionType.CardChange;
    private int _changeIndex;
    private bool _isSelectChanged;
    private bool _isGambled;

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
        _towerBuilder = FindObjectOfType<TowerBuilder>();
        _cardDrawer = new CardDrawer();
        _isGambled = false;
    }

    public void StartGamble(int gambleType)
    {
        // �̹� Gamble�� �����ϰ� �ִ� ���̶�� �������� �ʴ´�. (Ȥ�ø� ���� ����)
        if (_isGambled) return;

        // Gamble�� �����ϱ� ���� 100��带 �����ϰ� ���� �ʴٸ� �������� �ʴ´�.
        if (StageManager.instance.gold < 100)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughGold);
            return;
        }

        // �÷��̾��� ��忡�� 100��带 �����Ѵ�.
        StageManager.instance.gold -= 100;
        // ī�� �̴� ���� ���.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);

        if (gambleType == (int)GambleType.Tower)
        {
            // Gambler�� gamble type�� Tower�� ����
            _gambleType = GambleType.Tower;
        }
        else
        {
            // Gambler�� gamble type�� Mineral�� ����
            _gambleType = GambleType.Mineral;
        }

        // Gamble�� ���� ���� ���·� �ٲ۴�.
        _isGambled = true;

        // �̱� ��ư�� ȭ�鿡�� �����.
        _gambleUIController.HideGambleButtonUI();

        // ī�带 �̴´�.
        _cardDrawer.DrawCardAll();

        // �÷��̾� ȭ�鿡 ���� ���� ī�带 �����ش�.
        _gambleUIController.AllReverseCardFrontUI();
        // ī�� �ٲٱ� ��ɰ� ��Ŀī�� ��� ��ư�� Ȱ��ȭ �Ѵ�.
        _gambleUIController.ShowFunctionToggle();
        // ���� ī�� ����� ����.
        _gambleUIController.ShowResultUI();
    }

    public void ToggleButtonFunction()
    {
        if (buttonFunctionType == ButtonFunctionType.CardChange)
            buttonFunctionType = ButtonFunctionType.JokerCard;
        else
            buttonFunctionType = ButtonFunctionType.CardChange;

        //SoundManager.instance.PlaySFX("Button Toggle Sound");
    }

    public void ExecuteButtonFunction(int changeIndex)
    {
        if (_buttonFunctionType == ButtonFunctionType.CardChange)
            CardRandomChange(changeIndex);
        else
            ShowCardSelector(changeIndex);
    }

    private void CardRandomChange(int changeIndex)
    {
        // �÷��̾��� ChangeChance Ƚ���� 0 ���϶�� �������� �ʴ´�.
        if (StageManager.instance.changeChance <= 0)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughChangeChance);
            return;
        }

        // �÷��̾��� ChangeChance Ƚ���� 1 �����Ѵ�.
        StageManager.instance.changeChance--;
        // ī�� �̴� ���� ���.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);
        // ī�带 �ٲ۴�.
        _cardDrawer.ChangeRandomCard(changeIndex);

        // �ٲ� ī�带 �÷��̾�� �����ش�.
        _gambleUIController.ReverseCardFrountUI(changeIndex);
        // ���� ī�� ����� ����.
        _gambleUIController.ShowResultUI();
    }

    private void ShowCardSelector(int changeIndex)
    {
        // �÷��̾��� JokerCard ������ 0�� ���϶�� �������� �ʴ´�.
        if (StageManager.instance.jokerCard <= 0)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.NotEnoughJokerCard);
            return;
        }
        // �÷��̾ JokerCard�� �̹� ����� ���¶�� �������� �ʴ´�.
        else if(_isSelectChanged)
        {
            StageUIManager.instance.ShowSystemMessage(SystemMessage.MessageType.AlreadyUsedJokerCard);
            return;
        }

        // �̹� Card Selector�� Ȱ��ȭ �Ǿ��ִ� ��� �ٲ� ī���� index�� �ٲ۴�.
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
            _gambleUIController.DisableFunctionToggle();
            _cardSelector.gameObject.SetActive(true);
        }
    }
    
    public void HideCardSelector()
    {
        _gambleUIController.MarkCancelChangeCard(_changeIndex);
        _gambleUIController.EnableFunctionToggle();
        _cardSelector.gameObject.SetActive(false);
    }

    public void CardSelectChange(int cardIndex)
    {
        // ��Ŀī�� ���� 1�� ����
        StageManager.instance.jokerCard--;
        // ī�� �̴� ���� ���.
        SoundManager.instance.PlaySFX(SoundFileNameDictionary.cardGambleSound);

        HideCardSelector();
        // ���޹��� ī�� ��ȣ�� ī�带 �ٲ۴�.
        _cardDrawer.ChangeSelectCard(_changeIndex, cardIndex);
        // �ٲ� ī�带 �÷��̾�� �����ش�.
        _gambleUIController.ReverseCardFrountUI(_changeIndex);
        // ���� ī�� ����� ����.
        _gambleUIController.ShowResultUI();

        _isSelectChanged = true;
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
        _isSelectChanged = false;
        _isGambled = false;
        _cardDrawer.ResetDrawer();
        _gambleUIController.ResetGambleUI();

        if (_cardSelector.gameObject.activeSelf)
            HideCardSelector();
    }
}
