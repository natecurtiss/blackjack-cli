using static Blackjack.Cards;

namespace Blackjack;

public sealed class Deck
{
    readonly List<int> _cards = new();

    public Deck()
    {
        var suit = new[]
        {
            TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, 
            JACK, QUEEN, KING, ACE
        };
        foreach (var card in suit)
            for (var i = 0; i < 4; i++)
                _cards.Add(card);
    }

    public void Shuffle() => _cards.Shuffle();

    public int Take()
    {
        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}