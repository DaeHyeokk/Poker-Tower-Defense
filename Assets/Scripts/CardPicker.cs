using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    private const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_CARD = 52;
    /// <summary>
    /// enum 열거형 선언
    /// </summary>
    public enum Pattern { Spade, Heart, Diamond, Clover }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
    public enum PokerHand { Top, OnePair, TwoPair, Triple, Straight, Mountain, Flush, FullHouse, FourKind, StraightFlush }

    /// <summary>
    /// 뽑은 카드의 정보를 저장하는 구조체
    /// </summary>
    public struct Card
    {
        Pattern pattern;
        Number number;
    }

    /// <summary>
    /// 52장의 카드 스프라이트와 플레이어에게 미리 보여줄 타워 스프라이트
    /// 배열의 인덱스값을 기준으로 원하는 스프라이트에 접근하기 때문에 등록 순서 중요!!
    /// </summary>
    [SerializeField]
    private Sprite[] _cardSprites;
    [SerializeField]
    private Sprite _cardBackSprite;
    [SerializeField]
    private Sprite[] _towerSprites;

    /// <summary>
    /// _pickHand : 뽑은 카드의 족보값(?)을 저장하는 변수
    /// _pickCards : 뽑은 카드들의 정보를 비트마스킹 기법으로 저장하는 변수
    /// 카드가 총 52장이기 때문에 64bit 자료형인 long 을 사용함
    /// </summary>
    private PokerHand _pickHand;
    private long _pickCardsMasking;

    private Card[] _pickCards;
    private int[] _pickNumbers;
    private int[] _pickPatterns;

    public int[] pickNumbers => _pickNumbers;

    private void Awake()
    {
        _pickCardsMasking = 0;
    }

}
