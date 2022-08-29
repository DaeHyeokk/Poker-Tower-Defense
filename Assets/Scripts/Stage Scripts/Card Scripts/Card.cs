using System.Collections;

/// <summary>
/// 카드의 정보를 저장하는 클래스
/// </summary>
public class Card
{
    public const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_COUNT = 52;

    public enum Pattern { Clover, Diamond, Heart, Spade }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    private Pattern _pattern;
    private Number _number;
    private bool _isEmpty;

    public Pattern pattern => _pattern;
    public Number number => _number;
    public int index => (int)_pattern * MAX_NUMBER + (int)_number;
    public bool isEmpty => _isEmpty;

    public Card()
    {
        _isEmpty = true;
    }

    public void SetCard(int cardIndex)
    {
        _pattern = (Pattern)(cardIndex / MAX_NUMBER);
        _number = (Number)(cardIndex % MAX_NUMBER);
        _isEmpty = false;
    }
}


/*
 * File : Card.cs
 * First Update : 2022/04/27 WED 22:15
 * 카드의 무늬 및 숫자를 저장하는 클래스.
 * int형 데이터를 입력으로 받아 무늬와 숫자로 변환하여 저장한다.
 * 무늬와 숫자를 다시 int형 데이터로 변환할 수 있다.
 */