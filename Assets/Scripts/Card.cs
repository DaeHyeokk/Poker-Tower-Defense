using System.Collections;

/// <summary>
/// ī���� ������ �����ϴ� Ŭ����
/// </summary>
public class Card
{
    private const int MAX_NUMBER = 13;

    public enum Pattern { Spade, Heart, Diamond, Clover }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    private Pattern _pattern;
    private Number _number;
    private bool _isEmpty;

    public Pattern pattern => _pattern;
    public Number number => _number;
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

    public int GetIndex()
    {
        return (int)_pattern * MAX_NUMBER + (int)_number;
    }
}


/*
 * File : Card.cs
 * First Update : 2022/04/27 WED 22:15
 * ī���� ���� �� ���ڸ� �����ϴ� Ŭ����.
 * int�� �����͸� �Է����� �޾� ���̿� ���ڷ� ��ȯ�Ͽ� �����Ѵ�.
 * ���̿� ���ڸ� �ٽ� int�� �����ͷ� ��ȯ�� �� �ִ�.
 */