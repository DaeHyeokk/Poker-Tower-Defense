using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carddd
{
    public enum Suit { Spade, Heart, Diamond, Clova }
    public enum Number { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    private Suit _suit;
    private Number _number;

    public Suit suit => _suit;
    public Number number => _number;

    public Carddd(int pickNumber)
    {
        _suit = (Suit)(pickNumber / 12);
        _number = (Number)(pickNumber % 12);
    }
}
