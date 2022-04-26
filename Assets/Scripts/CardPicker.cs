using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    private const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_CARD = 52;
    /// <summary>
    /// enum ������ ����
    /// </summary>
    public enum Pattern { Spade, Heart, Diamond, Clover }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
    public enum PokerHand { Top, OnePair, TwoPair, Triple, Straight, Mountain, Flush, FullHouse, FourKind, StraightFlush }

    /// <summary>
    /// ���� ī���� ������ �����ϴ� ����ü
    /// </summary>
    public struct Card
    {
        Pattern pattern;
        Number number;
    }

    /// <summary>
    /// 52���� ī�� ��������Ʈ�� �÷��̾�� �̸� ������ Ÿ�� ��������Ʈ
    /// �迭�� �ε������� �������� ���ϴ� ��������Ʈ�� �����ϱ� ������ ��� ���� �߿�!!
    /// </summary>
    [SerializeField]
    private Sprite[] _cardSprites;
    [SerializeField]
    private Sprite _cardBackSprite;
    [SerializeField]
    private Sprite[] _towerSprites;

    /// <summary>
    /// _pickHand : ���� ī���� ������(?)�� �����ϴ� ����
    /// _pickCards : ���� ī����� ������ ��Ʈ����ŷ ������� �����ϴ� ����
    /// ī�尡 �� 52���̱� ������ 64bit �ڷ����� long �� �����
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
