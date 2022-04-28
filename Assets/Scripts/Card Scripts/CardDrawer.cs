using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawer : MonoBehaviour
{
    private const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_CARD = 52;
    public enum PokerHand { Top, OnePair, TwoPair, Triple, Straight, Mountain, Flush, FullHouse, FourKind, StraightFlush }

    private CardUIController _cardUIController;

    /// ���� ī���� ������(?)�� �����ϴ� ����.
    private PokerHand _drawHand;

    /// ���� ī����� ����� ��Ʈ����ŷ ������� �����ϴ� ����.
    /// ī�尡 �� 52���̱� ������ 64bit �ڷ����� long �� �����.
    private long _drawCardsMasking;

    /// ī�尡 ���� �������� ���θ� ��Ÿ���� ����
    private bool _isDraw;

    /// ���� ī����� ���� �� ���� ������ �����ϴ� ����.
    /// �÷��̾��� ȭ�鿡 ���� ������� ī�带 �����ֱ� ���� Card[] �迭.
    private Card[] _drawCards;
    private int[] _drawNumbers;
    private int[] _drawPatterns;

    public Card[] drawCards => _drawCards;
    public int[] drawNumbers => _drawNumbers;
    public int[] drawPatterns => _drawPatterns;
    public PokerHand drawHand => _drawHand;
    public bool isDraw => _isDraw;

    private void Awake()
    {
        _cardUIController = GetComponent<CardUIController>();

        _drawCards = new Card[5];
        _drawNumbers = new int[MAX_NUMBER];
        _drawPatterns = new int[MAX_PATTERN];
        _drawCardsMasking = 0;
        _isDraw = false;

        for(int i=0; i<5; i++)
            _drawCards[i] = new();
    }

    public void DrawCardAll()
    {
        // �÷��̾� ȭ�鿡 ���µ� ī�带 ��� �����´�.
        _cardUIController.AllDisableCardUI();
        // ī�� Draw ��ư�� ��Ȱ��ȭ �Ѵ�.
        _cardUIController.DisableDrawButtonUI();

        // ī�带 5�� �̴´�.
        for(int drawed = 0; drawed <5; drawed++)
            DrawCard(drawed);

        // ���� ������ ������Ʈ �Ѵ�.
        UpdateHandInfo();

        // �÷��̾� ȭ�鿡 ���� ���� ī�带 �����ش�.
        _cardUIController.AllEnableCardUI();
    }

    public void ChangeCard(int changeIndex)
    {
        // �÷��̾� ȭ�鿡 ���µ� ī�� �� �ٲ� ī�带 �����´�.
        _cardUIController.DisableCardUI(changeIndex);

        // ���ο� ī�带 �̰� ������ �����ϱ� ���� �ٲٱ� �� ī���� index������ �ӽ÷� �����صд�.
        int changeBitIndex = _drawCards[changeIndex].GetIndex();

        // ���ο� ī�带 �̴´�.
        DrawCard(changeIndex);

        // �ӽ÷� �����س��� index�� �̿��� ����ŷ�� ��Ʈ�� 0���� �ٲ��ش�.
        // ���� ���Ŀ� �����ϴ� ������ �ٲٷ��� �ϴ� ī�尡 �ٽ� ������ ���� ������ �ϱ� ����.
        _drawCardsMasking &= ~((long)1 << changeBitIndex);

        // ���� ������ ������Ʈ �Ѵ�.
        UpdateHandInfo();

        // �÷��̾� ȭ�鿡 ���� ���� ī�带 �����ش�.
        _cardUIController.EnableCardUI(changeIndex);
    }

    private void DrawCard(int index)
    {
        // 0~51�� ������ �������� �ϳ��� ����.
        int drawCardIndex = Random.Range(0, MAX_CARD);

        // ���� ���� ���� ī�尡 �̹� �̾Ҵ� ī���� ��� �ٽ� �̴´�.
        if(_drawCardsMasking == (_drawCardsMasking | ((long)1 << drawCardIndex)))
            DrawCard(index);
        else
        {
            // ���� ī���� index��° ��Ʈ�� ����ŷ�Ѵ�.
            _drawCardsMasking |= ((long)1 << drawCardIndex);
            // ���� ī�� ������ ����.
            AccumulateCardInfo(drawCardIndex, index);
        }
    }

    private void AccumulateCardInfo(int drawCardIndex, int index)
    {
        int patternIndex;
        int numberIndex;

        // ���� ���ο� ���� ������ ī�忡 �̹� �ٸ� ���� �ִٸ�,
        // ������ �������״� ���� ���ش�.
        if(!_drawCards[index].isEmpty)
        {
            patternIndex = (int)_drawCards[index].pattern;
            numberIndex = (int)_drawCards[index].number;
            _drawPatterns[patternIndex]--;
            _drawNumbers[numberIndex]--;
        }

        // ���ο� ī�� ������ �����ϰ�, �� ���� ������Ų��.
        _drawCards[index].SetCard(drawCardIndex);
        patternIndex = (int)_drawCards[index].pattern;
        numberIndex = (int)_drawCards[index].number;
        _drawPatterns[patternIndex]++;
        _drawNumbers[numberIndex]++;
    }

    private void UpdateHandInfo()
    {
        bool isOnePair = false, isTriple = false, isStraight = false, isMountain = false, isFlush = false;
        
        _drawHand = PokerHand.Top;

        // �����, �����, Ʈ����, ��ī��, ��Ʈ����Ʈ ���θ� üũ�Ѵ�.
        int straightCount = 0;
        for(int i=0; i<MAX_NUMBER; i++)
        {
            int cardCount = _drawNumbers[i];

            // ���� i�� ���� �̻� �ִ� ��� ��Ʈ����Ʈ ī��Ʈ�� 1 ������Ų��.
            // ���嵵 ���� ��� ��Ʈ����Ʈ ī��Ʈ�� 0���� ���½�Ų��.
            if (cardCount > 0)
                straightCount++;
            else
                straightCount = 0;

            // ���ӵǴ� 5���� ���ڰ� ���� ��� ��Ʈ����Ʈ ���� ����.
            if(straightCount == 5)
            {
                if (_drawHand < PokerHand.Straight)
                    _drawHand = PokerHand.Straight;
                isStraight = true;
            }

            switch(cardCount)
            {
                // ������� ��� ����
                case 2:
                    if (isTriple)
                    {
                        if (_drawHand < PokerHand.FullHouse)
                            _drawHand = PokerHand.FullHouse;
                    }
                    else if (isOnePair)
                    {
                        if (_drawHand < PokerHand.TwoPair)
                            _drawHand = PokerHand.TwoPair;
                    }
                    else
                    {
                        isOnePair = true;
                        if (_drawHand < PokerHand.OnePair)
                            _drawHand = PokerHand.OnePair;
                    }
                    break;

                // Ʈ������ ��� ����
                case 3:
                    if(isOnePair)
                    {
                        if (_drawHand < PokerHand.FullHouse)
                            _drawHand = PokerHand.FullHouse;
                    }
                    else
                    {
                        if (_drawHand < PokerHand.Triple)
                            _drawHand = PokerHand.Triple;
                        isTriple = true;
                    }
                    break;
                
                // ��ī���� ��� ����
                case 4:
                    if (_drawHand < PokerHand.FourKind)
                        _drawHand = PokerHand.FourKind;
                    break;
                    
            }
        }

        // Ace���� 2,3,4,5,6,7 .... J,Q,K ���� Ȯ���ϰ� ������ ��,
        // straightCount�� 4�� ��� 10, J, Q, K �� ���� ������ �� �� �ִ�.
        // ���� ����ƾ�� ���ɼ��� �����Ƿ� üũ�Ѵ�.
        if(straightCount == 4)
        {
            if (drawNumbers[(int)Card.Number.Ace] > 0)
            {
                if (_drawHand < PokerHand.Mountain)
                    _drawHand = PokerHand.Mountain;
                isMountain = true;
            }
        }

        // �÷��� ���θ� üũ�Ѵ�.
        for(int i=0; i<MAX_PATTERN; i++)
        {
            int cardCount = _drawPatterns[i];
            if(cardCount >= 5)
            {
                if (_drawHand < PokerHand.Flush)
                    _drawHand = PokerHand.Flush;
                isFlush = true;
            }
        }

        // ���������� ��Ʈ����Ʈ�� ����ƾ ���� �ϳ��� �÷����� true ���, ��Ʈ����Ʈ�÷����� �ȴ�.
        if((isStraight || isMountain) && isFlush)
        {
            if (_drawHand < PokerHand.StraightFlush)
                _drawHand = PokerHand.StraightFlush;
        }
    }

    public void ReadyBuildTower()
    {
        // ī�带 ���� ���·� �ٲ۴�.
        _isDraw = true;
    }

    public void ResetDrawer()
    {
        // ������ ����ŷ�ߴ� ������ �ʱ�ȭ�Ѵ�.
        _drawCardsMasking = 0;
        // ī�带 �Ȼ��� ���·� �ǵ�����.
        _isDraw = false;

        // ī�带 ��� �����´�.
        _cardUIController.AllDisableCardUI();
        // Ÿ���� Get �ϴ� ��ư�� ��Ȱ��ȭ �Ѵ�.
        _cardUIController.DisableGetButtonUI();
        // ������� ������ ��Ÿ���� �ؽ�Ʈ�� ��Ȱ��ȭ �Ѵ�.
        _cardUIController.DisableHandTextUI();
        // ī�� Draw ��ư�� Ȱ��ȭ �Ѵ�.
        _cardUIController.EnableDrawButtonUI();
    }
}

/*
 * File : CardDrawer.cs
 * First Update : 2022/04/27 WED 22:20
 * ī�带 �̰�, ���� ī���� ������ �Ǻ��ϴ� ��ũ��Ʈ.
 * �ټ����� ī�带 �̴� �۾��� ī�� ������ �ٲ� �̴� �۾�, 
 * ���� ī�带 �������� ������� �и� �Ǻ��ϴ� �۾��� �����Ѵ�.
 * �̹� ���� ī�带 �ٽ� ���� �ʱ� ���� ������� ���� Cards[] �迭�� Ž���ϴ� ����� �ƴ�,
 * ��Ʈ����ŷ ����� ���� ��Ʈ�������� O(1) �߿����� ���� ������ üũ�� �� �ֵ��� �����Ͽ���.
 * ī�尡 5������ ���ѵǾ� �־� �ΰ��ӿ� ������ ��ĥ ������ ���� ����� ����ϱ� ��ư����� �� ������ ���Ǹ� �ΰ� �ִ�. 
 */
