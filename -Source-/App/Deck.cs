using static Blackjack.Card;

namespace Blackjack;

public sealed class Deck
{
    readonly List<Card> _cards = new();

    public Deck() => Reset();
    
    public void Reset()
    {
        _cards.Clear();
        const int suits = 4;
        var suit = new Card[]
        {
            new("Two", 2),
            new("Three", 3),
            new("Four", 4),
            new("Five", 5),
            new("Six", 6),
            new("Seven", 7),
            new("Eight", 8),
            new("Nine", 9),
            new("Ten", 10),
            new("Jack", 10),
            new("Queen", 10),
            new("King", 10),
            new("Ace", 11),
        };
        foreach (var card in suit)
            for (var i = 0; i < suits; i++)
                _cards.Add(card);
    }

    public void Shuffle() => _cards.Shuffle();

    public Card Top()
    {
        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}