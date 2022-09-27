/// <summary>
/// ī���� ������ �����ϴ� Ŭ����
/// </summary>
public class Card
{
    public const int MAX_NUMBER = 13, MAX_PATTERN = 4, MAX_COUNT = 52, COLOR_JOKER_INDEX = 54, MONOCHROME_JOKER_INDEX = 55;

    public enum Pattern { Clover, Diamond, Heart, Spade, Joker }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }
    public enum JokerType { Color, Monochrome }

    private Pattern _pattern;
    private Number _number;
    private JokerType _jokerType;
    private bool _isJoker;

    public Pattern pattern => _pattern;
    public Number number => _number;
    public int index 
    {
        get 
        {
            if(_isJoker)
            {
                if (_jokerType == JokerType.Color)
                    return COLOR_JOKER_INDEX;
                else
                    return MONOCHROME_JOKER_INDEX;
            }
            else
                return (int)_pattern * MAX_NUMBER + (int)_number;
        }
    }
    public bool isJoker => _isJoker;


    public void SetCard(int cardIndex)
    {
        if (cardIndex == COLOR_JOKER_INDEX)
        {
            _isJoker = true;
            _jokerType = JokerType.Color;
        }
        else if (cardIndex == MONOCHROME_JOKER_INDEX)
        {
            _isJoker = true;
            _jokerType = JokerType.Monochrome;
        }
        else
        {
            _isJoker = false;
            _pattern = (Pattern)(cardIndex / MAX_NUMBER);
            _number = (Number)(cardIndex % MAX_NUMBER);
        }
    }
}


/*
 * File : Card.cs
 * First Update : 2022/04/27 WED 22:15
 * ī���� ���� �� ���ڸ� �����ϴ� Ŭ����.
 * int�� �����͸� �Է����� �޾� ���̿� ���ڷ� ��ȯ�Ͽ� �����Ѵ�.
 * ���̿� ���ڸ� �ٽ� int�� �����ͷ� ��ȯ�� �� �ִ�.
 * 
 * Update : 2022/09/23 FRI 03:27
 * ��Ŀī�� �߰�.
 */