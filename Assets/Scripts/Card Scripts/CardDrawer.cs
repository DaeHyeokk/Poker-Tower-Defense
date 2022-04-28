using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawer : MonoBehaviour
{
    private const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_CARD = 52;
    public enum PokerHand { Top, OnePair, TwoPair, Triple, Straight, Mountain, Flush, FullHouse, FourKind, StraightFlush }

    private CardUIController _cardUIController;

    /// 뽑은 카드의 족보값(?)을 저장하는 변수.
    private PokerHand _drawHand;

    /// 뽑은 카드들의 목록을 비트마스킹 기법으로 저장하는 변수.
    /// 카드가 총 52장이기 때문에 64bit 자료형인 long 을 사용함.
    private long _drawCardsMasking;

    /// 카드가 뽑힌 상태인지 여부를 나타내는 변수
    private bool _isDraw;

    /// 뽑은 카드들의 무늬 및 숫자 정보를 저장하는 변수.
    /// 플레이어의 화면에 뽑은 순서대로 카드를 보여주기 위한 Card[] 배열.
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
        // 플레이어 화면에 오픈된 카드를 모두 뒤집는다.
        _cardUIController.AllDisableCardUI();
        // 카드 Draw 버튼을 비활성화 한다.
        _cardUIController.DisableDrawButtonUI();

        // 카드를 5장 뽑는다.
        for(int drawed = 0; drawed <5; drawed++)
            DrawCard(drawed);

        // 족보 정보를 업데이트 한다.
        UpdateHandInfo();

        // 플레이어 화면에 새로 뽑은 카드를 보여준다.
        _cardUIController.AllEnableCardUI();
    }

    public void ChangeCard(int changeIndex)
    {
        // 플레이어 화면에 오픈된 카드 중 바꿀 카드를 뒤집는다.
        _cardUIController.DisableCardUI(changeIndex);

        // 새로운 카드를 뽑고 정보를 저장하기 전에 바꾸기 전 카드의 index정보를 임시로 저장해둔다.
        int changeBitIndex = _drawCards[changeIndex].GetIndex();

        // 새로운 카드를 뽑는다.
        DrawCard(changeIndex);

        // 임시로 저장해놓은 index를 이용해 마스킹한 비트를 0으로 바꿔준다.
        // 뽑은 이후에 수행하는 이유는 바꾸려고 하는 카드가 다시 뽑히는 일이 없도록 하기 위함.
        _drawCardsMasking &= ~((long)1 << changeBitIndex);

        // 족보 정보를 업데이트 한다.
        UpdateHandInfo();

        // 플레이어 화면에 새로 뽑은 카드를 보여준다.
        _cardUIController.EnableCardUI(changeIndex);
    }

    private void DrawCard(int index)
    {
        // 0~51의 숫자중 랜덤으로 하나를 선택.
        int drawCardIndex = Random.Range(0, MAX_CARD);

        // 만약 지금 뽑은 카드가 이미 뽑았던 카드일 경우 다시 뽑는다.
        if(_drawCardsMasking == (_drawCardsMasking | ((long)1 << drawCardIndex)))
            DrawCard(index);
        else
        {
            // 뽑은 카드의 index번째 비트를 마스킹한다.
            _drawCardsMasking |= ((long)1 << drawCardIndex);
            // 뽑은 카드 정보를 저장.
            AccumulateCardInfo(drawCardIndex, index);
        }
    }

    private void AccumulateCardInfo(int drawCardIndex, int index)
    {
        int patternIndex;
        int numberIndex;

        // 만약 새로운 값을 저장할 카드에 이미 다른 값이 있다면,
        // 이전에 누적시켰던 값을 빼준다.
        if(!_drawCards[index].isEmpty)
        {
            patternIndex = (int)_drawCards[index].pattern;
            numberIndex = (int)_drawCards[index].number;
            _drawPatterns[patternIndex]--;
            _drawNumbers[numberIndex]--;
        }

        // 새로운 카드 정보를 저장하고, 그 값을 누적시킨다.
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

        // 원페어, 투페어, 트리플, 포카드, 스트레이트 여부를 체크한다.
        int straightCount = 0;
        for(int i=0; i<MAX_NUMBER; i++)
        {
            int cardCount = _drawNumbers[i];

            // 숫자 i가 한장 이상 있는 경우 스트레이트 카운트를 1 증가시킨다.
            // 한장도 없을 경우 스트레이트 카운트를 0으로 리셋시킨다.
            if (cardCount > 0)
                straightCount++;
            else
                straightCount = 0;

            // 연속되는 5개의 숫자가 있을 경우 스트레이트 조건 성립.
            if(straightCount == 5)
            {
                if (_drawHand < PokerHand.Straight)
                    _drawHand = PokerHand.Straight;
                isStraight = true;
            }

            switch(cardCount)
            {
                // 원페어일 경우 실행
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

                // 트리플일 경우 실행
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
                
                // 포카드일 경우 실행
                case 4:
                    if (_drawHand < PokerHand.FourKind)
                        _drawHand = PokerHand.FourKind;
                    break;
                    
            }
        }

        // Ace부터 2,3,4,5,6,7 .... J,Q,K 까지 확인하고 나왔을 때,
        // straightCount가 4일 경우 10, J, Q, K 가 연속 됐음을 알 수 있다.
        // 따라서 마운틴의 가능성이 있으므로 체크한다.
        if(straightCount == 4)
        {
            if (drawNumbers[(int)Card.Number.Ace] > 0)
            {
                if (_drawHand < PokerHand.Mountain)
                    _drawHand = PokerHand.Mountain;
                isMountain = true;
            }
        }

        // 플러쉬 여부를 체크한다.
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

        // 마지막으로 스트레이트나 마운틴 둘중 하나와 플러쉬가 true 라면, 스트레이트플러쉬가 된다.
        if((isStraight || isMountain) && isFlush)
        {
            if (_drawHand < PokerHand.StraightFlush)
                _drawHand = PokerHand.StraightFlush;
        }
    }

    public void ReadyBuildTower()
    {
        // 카드를 뽑은 상태로 바꾼다.
        _isDraw = true;
    }

    public void ResetDrawer()
    {
        // 이전에 마스킹했던 정보를 초기화한다.
        _drawCardsMasking = 0;
        // 카드를 안뽑은 상태로 되돌린다.
        _isDraw = false;

        // 카드를 모두 뒤집는다.
        _cardUIController.AllDisableCardUI();
        // 타워를 Get 하는 버튼을 비활성화 한다.
        _cardUIController.DisableGetButtonUI();
        // 만들어진 족보를 나타내는 텍스트를 비활성화 한다.
        _cardUIController.DisableHandTextUI();
        // 카드 Draw 버튼을 활성화 한다.
        _cardUIController.EnableDrawButtonUI();
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
 */
