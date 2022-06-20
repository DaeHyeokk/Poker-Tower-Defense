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

    // ������ ����(Ÿ������, �̳׶��̱�) ī�带 �̴����� ��Ÿ���� ����
    private GambleType _gambleType;
    // ȭ�鿡�� ī�� UI ���� ��ư�� ������ �� �����ϴ� ����� ��Ÿ���� ���� (�ʱ� �������� Card Change ���)
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
        // �̹� Gamble�� �����ϰ� �ִ� ���̶�� �������� �ʴ´�. (Ȥ�ø� ���� ����)
        if (_isGambling) return;

        // Gamble�� �����ϱ� ���� 100��带 �����ϰ� ���� �ʴٸ� �������� �ʴ´�.
        if (GameManager.instance.gold < 100)
        {
            UIManager.instance.ShowSystemMessage("��尡 �����մϴ�.");
            return;
        }

        // �÷��̾��� ��忡�� 100��带 �����Ѵ�.
        GameManager.instance.gold -= 100;

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
        _isGambling = true;

        // �̱� ��ư�� ȭ�鿡�� �����.
        _gambleUIController.HideGambleButtonUI();

        // ī�带 �̴´�.
        _cardDrawer.DrawCardAll();

        // �÷��̾� ȭ�鿡 ���� ���� ī�带 �����ش�.
        _gambleUIController.AllReverseCardFrontUI();
        _gambleUIController.ShowFunctionToggleButton();
        // ���� ī�� ����� ����.
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
        // �÷��̾��� ChangeChance Ƚ���� 0 ���϶�� �������� �ʴ´�.
        if (GameManager.instance.changeChance <= 0)
        {
            UIManager.instance.ShowSystemMessage("ī�� ��ȯ���� �����մϴ�.");
            return;
        }

        // �÷��̾��� ChangeChance Ƚ���� 1 �����Ѵ�.
        GameManager.instance.changeChance--;

        _changeIndex = changeIndex;

        // �÷��̾� ȭ�鿡 ���µ� ī�� �� �ٲ� ī�带 �����´�.
        _gambleUIController.ReverseCardBackUI(_changeIndex);

        // ī�带 �ٲ۴�.
        _cardDrawer.ChangeRandomCard(_changeIndex);

        // �ٲ� ī�带 �÷��̾�� �����ش�.
        _gambleUIController.ReverseCardFrountUI(changeIndex);
        // ���� ī�� ����� ����.
        _gambleUIController.ShowResultUI();
    }

    private void UseJokerCard(int cardIndex)
    {
        // �÷��̾��� JokerCard ������ 0�� ���϶�� �������� �ʴ´�.
        if (GameManager.instance.jokerCard <= 0)
        {
            UIManager.instance.ShowSystemMessage("��Ŀ ī�尡 �����մϴ�.");
            return;
        }

    }

    private void CardSelectChange(int changeIndex)
    {
        // �÷��̾��� JokerCard ������ 0�� ���϶�� �������� �ʴ´�.
        if (GameManager.instance.jokerCard <= 0)
        {
            UIManager.instance.ShowSystemMessage("��Ŀ ī�尡 �����մϴ�.");
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
