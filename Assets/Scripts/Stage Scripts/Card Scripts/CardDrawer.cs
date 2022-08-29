using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokerHand { 탑, 원페어, 투페어, 트리플, 스트레이트, 마운틴, 플러쉬, 풀하우스, 포카인드, 스트레이트플러쉬 }
public class CardDrawer
{
    // 뽑은 카드의 족보값(?)을 저장하는 변수.
    private PokerHand _drawHand;

    // 뽑은 카드들의 목록을 비트마스킹 기법으로 저장하는 변수.
    // 카드가 총 52장이기 때문에 64bit 자료형인 long 을 사용함.
    private long _drawCardsMasking;


    // 뽑은 카드들의 무늬 및 숫자 정보를 저장하는 변수.
    // 플레이어의 화면에 뽑은 순서대로 카드를 보여주기 위한 Card[] 배열.
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
        // 카드를 7장 뽑는다.
        for (int drawed = 0; drawed < StageManager.instance.pokerCount; drawed++)
            DrawCard(drawed);

        // 족보 정보를 업데이트 한다.
        UpdateHandInfo();
    }

    public void ChangeRandomCard(int changeIndex)
    {
        // 새로운 카드를 뽑고 정보를 저장하기 전에 바꾸기 전 카드의 index정보를 임시로 저장해둔다.
        int changeBitIndex = _drawCards[changeIndex].index;

        // 새로운 카드를 뽑는다.
        DrawCard(changeIndex);

        // 임시로 저장해놓은 index를 이용해 마스킹한 비트를 끈다.
        // 뽑은 이후에 수행하는 이유는 바꾸려고 하는 카드가 다시 뽑히는 일이 없도록 하기 위함.
        _drawCardsMasking &= ~((long)1 << changeBitIndex);

        // 족보 정보를 업데이트 한다.
        UpdateHandInfo();
    }

    public void ChangeSelectCard(int changeIndex, int cardIndex)
    {
        // 바꾸고자 하는 카드의 index번째 비트를 끈다.
        _drawCardsMasking &= ~((long)1 << _drawCards[changeIndex].index);
        // 새로 바꾼 카드의 index번째 비트를 켠다.
        _drawCardsMasking |= ((long)1 << cardIndex);

        // 바꾼 카드 정보를 저장
        _drawCards[changeIndex].SetCard(cardIndex);

        // 족보 정보를 업데이트 한다.
        UpdateHandInfo();
    }

    private void DrawCard(int index)
    {
        // 0~51의 숫자중 랜덤으로 하나를 선택.
        int drawCardIndex = Random.Range(0, Card.MAX_COUNT);

        // 만약 지금 뽑은 카드가 이미 뽑았던 카드일 경우 다시 뽑는다.
        if (_drawCardsMasking == (_drawCardsMasking | ((long)1 << drawCardIndex)))
            DrawCard(index);
        else
        {
            // 뽑은 카드의 index번째 비트를 켠다.
            _drawCardsMasking |= ((long)1 << drawCardIndex);
            // 뽑은 카드 정보를 저장.
            _drawCards[index].SetCard(drawCardIndex);
        }
    }

    private void UpdateHandInfo()
    {
        bool isOnePair = false, isTriple = false;
        int cardCount;
        _drawHand = PokerHand.탑;

        // 원페어, 투페어, 트리플, 포카드, 스트레이트 여부를 체크한다.
        int straightCount = 0;
        for (int number = 0; number < Card.MAX_NUMBER; number++)
        {
            cardCount = 0;
            for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
                if ((((long)1 << (Card.MAX_NUMBER * pattern + number) & _drawCardsMasking)) != 0)
                    cardCount++;

            // 숫자 i가 한장 이상 있는 경우 스트레이트 카운트를 1 증가시킨다.
            // 한장도 없을 경우 스트레이트 카운트를 0으로 리셋시킨다.
            if (cardCount > 0)
                straightCount++;
            else
                straightCount = 0;

            // 연속되는 5개의 숫자가 있을 경우 스트레이트 조건 성립.
            if (straightCount == 5)
                UpdateHand(PokerHand.스트레이트);

            switch (cardCount)
            {
                // 원페어일 경우 실행
                case 2:
                    if (isTriple)
                    {
                        UpdateHand(PokerHand.풀하우스);
                    }
                    else if (isOnePair)
                    {
                        UpdateHand(PokerHand.투페어);
                    }
                    else
                    {
                        isOnePair = true;
                        UpdateHand(PokerHand.원페어);
                    }
                    break;

                // 트리플일 경우 실행
                case 3:
                    if (isOnePair || isTriple)
                    {
                        UpdateHand(PokerHand.풀하우스);
                    }
                    else
                    {
                        isTriple = true;
                        UpdateHand(PokerHand.트리플);
                    }
                    break;

                // 포카드일 경우 실행
                case 4:
                    UpdateHand(PokerHand.포카인드);
                    break;

            }
        }

        // Ace부터 2,3,4,5,6,7 .... J,Q,K 까지 확인하고 나왔을 때,
        // straightCount가 4일 경우 10, J, Q, K 가 연속 됐음을 알 수 있다.
        // 따라서 마운틴의 가능성이 있으므로 체크한다.
        if (straightCount == 4)
        {
            for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
                if ((((long)1 << (pattern * Card.MAX_NUMBER)) & _drawCardsMasking) != 0)
                    UpdateHand(PokerHand.마운틴);
        }

        // 플러쉬와 스트레이트 플러쉬 여부를 체크한다.
        for (int pattern = 0; pattern < Card.MAX_PATTERN; pattern++)
        {
            bool isStraight = false;
            straightCount = 0;
            cardCount = 0;

            for (int number = 0; number < Card.MAX_NUMBER; number++)
            {
                // 현재 카드 index번쨰의 비트를 켜고 뽑은 카드와 AND 연산한 결과가 0이 아니라면 (즉, 카드가 존재한다면)
                if ((((long)1 << (pattern * Card.MAX_NUMBER + number)) & _drawCardsMasking) != 0)
                {
                    // 플러쉬와 스트레이트 카운트를 증가시킨다.
                    cardCount++;
                    straightCount++;

                    if(straightCount >= 5)
                        isStraight = true;
                }
                // 카드가 존재하지 않는다면 스트레이트 카운트를 0으로 초기화 한다.
                else
                    straightCount = 0;
            }

            // Ace,2,3,4 ... K 까지 탐색한 결과 카드카운트가 5 이상이라면 플러쉬이다.
            if (cardCount >= 5)
            {
                // 만약 스트레이트 카운트가 4 라면 마운틴의 가능성이 있으므로 Ace를 확인한다.
                if (straightCount == 4)
                    if ((((long)1 << (pattern * Card.MAX_NUMBER)) & _drawCardsMasking) != 0)
                        straightCount++;

                // 만약 스트레이트 카운트도 5 이상이라면 스트레이트 플러쉬이다.
                if (isStraight || straightCount >= 5)
                    UpdateHand(PokerHand.스트레이트플러쉬);
                else
                    UpdateHand(PokerHand.플러쉬);
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
        // 이전에 마스킹했던 정보를 초기화한다.
        _drawCardsMasking = 0;
    }
}

/*
 * File : CardDrawer.cs
 * First Update : 2022/04/27 WED 22:20
 * 카드를 뽑고, 뽑은 카드의 족보를 판별하는 스크립트.
 * 다섯장의 카드를 뽑는 작업과 카드 한장을 바꿔 뽑는 작업, 
 * 뽑은 카드를 바탕으로 만들어진 패를 판별하는 작업을 수행한다.
 * 이미 뽑은 카드를 다시 뽑지 않기 위한 방법으로 뽑은 Cards[] 배열을 탐색하는 방법이 아닌,
 * 비트마스킹 기법을 통해 비트연산으로 O(1) 중에서도 아주 빠르게 체크할 수 있도록 구현하였다.
 * 카드가 5장으로 제한되어 있어 인게임에 영향을 미칠 정도의 성능 향상은 기대하기 어렵겠지만 그 목적에 의의를 두고 있다. 
 * 
 * Update : 2022/05/02 MON 01:58
 * 카드를 5장 뽑는 방식에서 7장 뽑는 방식으로 변경.
 * 카드를 7장 뽑기 때문에 스트레이트와 플러쉬이면서 스트레이트 플러쉬가 안되는 경우가 생길 수 있게 되어 이를 구체적으로 확인하는 로직으로 변경.
 * 스트레이트 플러쉬인지 확인하기 위해선 플레이어가 뽑은 카드의 정확한 정보를 가지고 비교 해야함. 비트마스킹 기법을 활용하여 구현하였음.
 * 
 * Update : 2022/05/02 MON 15:27
 * 스트레이트 플러쉬만 비트마스킹 기법을 적용했던 것에서 모든 족보를 판별하는데 비트마스킹 기법을 적용함.
 *     Why? 기존 방식은 뽑은 카드의 무늬 및 숫자 개수를 캐싱하기 위해 추가적인 int[] 배열이 필요했음. 
 *     비트마스킹 기법을 적용하면 뽑은 카드를 저장하는 정수 데이터 하나만 가지고도 족보를 판별할 수 있기 때문에 메모리 공간을 절약할 수 있겠다 판단.
 *     또한 가장 많이 호출될 것으로 예상되는 메서드인 만큼 최대한 빠르게 실행되게 하기 위한 고민을 했는데 
 *     컴퓨터가 가장 빨리 수행한다는 비트 연산을 이용하면 시간복잡도를 더 줄일 수 있을것이라 판단하였음.
 *     
 *     그 결과 실제로 체감이 될 정도로 수행속도가 빨라지게 되었음 !!
 *     
 * Update : 2022/06/15 WED
 * 스트레이트 플러쉬와 풀하우스를 제대로 판별하지 못하는 버그 수정.
 */
