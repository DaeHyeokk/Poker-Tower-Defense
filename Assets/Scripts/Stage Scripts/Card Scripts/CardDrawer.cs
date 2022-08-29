using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokerHand { ž, �����, �����, Ʈ����, ��Ʈ����Ʈ, ����ƾ, �÷���, Ǯ�Ͽ콺, ��ī�ε�, ��Ʈ����Ʈ�÷��� }
public class CardDrawer
{
    // ���� ī���� ������(?)�� �����ϴ� ����.
    private PokerHand _drawHand;

    // ���� ī����� ����� ��Ʈ����ŷ ������� �����ϴ� ����.
    // ī�尡 �� 52���̱� ������ 64bit �ڷ����� long �� �����.
    private long _drawCardsMasking;


    // ���� ī����� ���� �� ���� ������ �����ϴ� ����.
    // �÷��̾��� ȭ�鿡 ���� ������� ī�带 �����ֱ� ���� Card[] �迭.
    private Card[] _drawCards;

    public Card[] drawCards => _drawCards;
    public PokerHand drawHand => _drawHand;

    public CardDrawer()
    {
        _drawCardsMasking = 0;
        _drawCards = new Card[StageManager.instance.pokerCount];

        for (int i = 0; i < StageManager.instance.pokerCount; i++)
        {
            _drawCards[i] = new();
        }
    }

    public void DrawCardAll()
    {
        // ī�带 7�� �̴´�.
        for (int drawed = 0; drawed < StageManager.instance.pokerCount; drawed++)
            DrawCard(drawed);

        // ���� ������ ������Ʈ �Ѵ�.
        UpdateHandInfo();
    }

    public void ChangeRandomCard(int changeIndex)
    {
        // ���ο� ī�带 �̰� ������ �����ϱ� ���� �ٲٱ� �� ī���� index������ �ӽ÷� �����صд�.
        int changeBitIndex = _drawCards[changeIndex].index;

        // ���ο� ī�带 �̴´�.
        DrawCard(changeIndex);

        // �ӽ÷� �����س��� index�� �̿��� ����ŷ�� ��Ʈ�� ����.
        // ���� ���Ŀ� �����ϴ� ������ �ٲٷ��� �ϴ� ī�尡 �ٽ� ������ ���� ������ �ϱ� ����.
        _drawCardsMasking &= ~((long)1 << changeBitIndex);

        // ���� ������ ������Ʈ �Ѵ�.
        UpdateHandInfo();
    }

    public void ChangeSelectCard(int changeIndex, int cardIndex)
    {
        // �ٲٰ��� �ϴ� ī���� index��° ��Ʈ�� ����.
        _drawCardsMasking &= ~((long)1 << _drawCards[changeIndex].index);
        // ���� �ٲ� ī���� index��° ��Ʈ�� �Ҵ�.
        _drawCardsMasking |= ((long)1 << cardIndex);

        // �ٲ� ī�� ������ ����
        _drawCards[changeIndex].SetCard(cardIndex);

        // ���� ������ ������Ʈ �Ѵ�.
        UpdateHandInfo();
    }

    private void DrawCard(int index)
    {
        // 0~51�� ������ �������� �ϳ��� ����.
        int drawCardIndex = Random.Range(0, Card.MAX_COUNT);

        // ���� ���� ���� ī�尡 �̹� �̾Ҵ� ī���� ��� �ٽ� �̴´�.
        if (_drawCardsMasking == (_drawCardsMasking | ((long)1 << drawCardIndex)))
            DrawCard(index);
        else
        {
            // ���� ī���� index��° ��Ʈ�� �Ҵ�.
            _drawCardsMasking |= ((long)1 << drawCardIndex);
            // ���� ī�� ������ ����.
            _drawCards[index].SetCard(drawCardIndex);
        }
    }

    private void UpdateHandInfo()
    {
        bool isOnePair = false, isTriple = false;
        int cardCount;
        _drawHand = PokerHand.ž;

        // �����, �����, Ʈ����, ��ī��, ��Ʈ����Ʈ ���θ� üũ�Ѵ�.
        int straightCount = 0;
        for (int number = 0; number < Card.MAX_NUMBER; number++)
        {
            cardCount = 0;
            for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
                if ((((long)1 << (Card.MAX_NUMBER * pattern + number) & _drawCardsMasking)) != 0)
                    cardCount++;

            // ���� i�� ���� �̻� �ִ� ��� ��Ʈ����Ʈ ī��Ʈ�� 1 ������Ų��.
            // ���嵵 ���� ��� ��Ʈ����Ʈ ī��Ʈ�� 0���� ���½�Ų��.
            if (cardCount > 0)
                straightCount++;
            else
                straightCount = 0;

            // ���ӵǴ� 5���� ���ڰ� ���� ��� ��Ʈ����Ʈ ���� ����.
            if (straightCount == 5)
                UpdateHand(PokerHand.��Ʈ����Ʈ);

            switch (cardCount)
            {
                // ������� ��� ����
                case 2:
                    if (isTriple)
                    {
                        UpdateHand(PokerHand.Ǯ�Ͽ콺);
                    }
                    else if (isOnePair)
                    {
                        UpdateHand(PokerHand.�����);
                    }
                    else
                    {
                        isOnePair = true;
                        UpdateHand(PokerHand.�����);
                    }
                    break;

                // Ʈ������ ��� ����
                case 3:
                    if (isOnePair || isTriple)
                    {
                        UpdateHand(PokerHand.Ǯ�Ͽ콺);
                    }
                    else
                    {
                        isTriple = true;
                        UpdateHand(PokerHand.Ʈ����);
                    }
                    break;

                // ��ī���� ��� ����
                case 4:
                    UpdateHand(PokerHand.��ī�ε�);
                    break;

            }
        }

        // Ace���� 2,3,4,5,6,7 .... J,Q,K ���� Ȯ���ϰ� ������ ��,
        // straightCount�� 4�� ��� 10, J, Q, K �� ���� ������ �� �� �ִ�.
        // ���� ����ƾ�� ���ɼ��� �����Ƿ� üũ�Ѵ�.
        if (straightCount == 4)
        {
            for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
                if ((((long)1 << (pattern * Card.MAX_NUMBER)) & _drawCardsMasking) != 0)
                    UpdateHand(PokerHand.����ƾ);
        }

        // �÷����� ��Ʈ����Ʈ �÷��� ���θ� üũ�Ѵ�.
        for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
        {
            bool isStraight = false;
            straightCount = 0;
            cardCount = 0;

            for (int number = 0; number < Card.MAX_NUMBER; number++)
            {
                // ���� ī�� index������ ��Ʈ�� �Ѱ� ���� ī��� AND ������ ����� 0�� �ƴ϶�� (��, ī�尡 �����Ѵٸ�)
                if ((((long)1 << (pattern * Card.MAX_NUMBER + number)) & _drawCardsMasking) != 0)
                {
                    // �÷����� ��Ʈ����Ʈ ī��Ʈ�� ������Ų��.
                    cardCount++;
                    straightCount++;

                    if(straightCount >= 5)
                        isStraight = true;
                }
                // ī�尡 �������� �ʴ´ٸ� ��Ʈ����Ʈ ī��Ʈ�� 0���� �ʱ�ȭ �Ѵ�.
                else
                    straightCount = 0;
            }

            // Ace,2,3,4 ... K ���� Ž���� ��� ī��ī��Ʈ�� 5 �̻��̶�� �÷����̴�.
            if (cardCount >= 5)
            {
                // ���� ��Ʈ����Ʈ ī��Ʈ�� 4 ��� ����ƾ�� ���ɼ��� �����Ƿ� Ace�� Ȯ���Ѵ�.
                if (straightCount == 4)
                    if ((((long)1 << (pattern * Card.MAX_NUMBER)) & _drawCardsMasking) != 0)
                        straightCount++;

                // ���� ��Ʈ����Ʈ ī��Ʈ�� 5 �̻��̶�� ��Ʈ����Ʈ �÷����̴�.
                if (isStraight || straightCount >= 5)
                    UpdateHand(PokerHand.��Ʈ����Ʈ�÷���);
                else
                    UpdateHand(PokerHand.�÷���);
            }
        }
    }

    private void UpdateHand(PokerHand pokerHand)
    {
        if (_drawHand < pokerHand)
            _drawHand = pokerHand;
    }

    public void ResetDrawer()
    {
        // ������ ����ŷ�ߴ� ������ �ʱ�ȭ�Ѵ�.
        _drawCardsMasking = 0;
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
 * 
 * Update : 2022/05/02 MON 01:58
 * ī�带 5�� �̴� ��Ŀ��� 7�� �̴� ������� ����.
 * ī�带 7�� �̱� ������ ��Ʈ����Ʈ�� �÷����̸鼭 ��Ʈ����Ʈ �÷����� �ȵǴ� ��찡 ���� �� �ְ� �Ǿ� �̸� ��ü������ Ȯ���ϴ� �������� ����.
 * ��Ʈ����Ʈ �÷������� Ȯ���ϱ� ���ؼ� �÷��̾ ���� ī���� ��Ȯ�� ������ ������ �� �ؾ���. ��Ʈ����ŷ ����� Ȱ���Ͽ� �����Ͽ���.
 * 
 * Update : 2022/05/02 MON 15:27
 * ��Ʈ����Ʈ �÷����� ��Ʈ����ŷ ����� �����ߴ� �Ϳ��� ��� ������ �Ǻ��ϴµ� ��Ʈ����ŷ ����� ������.
 *     Why? ���� ����� ���� ī���� ���� �� ���� ������ ĳ���ϱ� ���� �߰����� int[] �迭�� �ʿ�����. 
 *     ��Ʈ����ŷ ����� �����ϸ� ���� ī�带 �����ϴ� ���� ������ �ϳ��� ������ ������ �Ǻ��� �� �ֱ� ������ �޸� ������ ������ �� �ְڴ� �Ǵ�.
 *     ���� ���� ���� ȣ��� ������ ����Ǵ� �޼����� ��ŭ �ִ��� ������ ����ǰ� �ϱ� ���� ����� �ߴµ� 
 *     ��ǻ�Ͱ� ���� ���� �����Ѵٴ� ��Ʈ ������ �̿��ϸ� �ð����⵵�� �� ���� �� �������̶� �Ǵ��Ͽ���.
 *     
 *     �� ��� ������ ü���� �� ������ ����ӵ��� �������� �Ǿ��� !!
 *     
 * Update : 2022/06/15 WED
 * ��Ʈ����Ʈ �÷����� Ǯ�Ͽ콺�� ����� �Ǻ����� ���ϴ� ���� ����.
 */
