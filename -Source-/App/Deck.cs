using static Blackjack.Card;

namespace Blackjack;

public sealed class Deck
{
    readonly List<Card> _cards = new();

    public Deck()
    {
        const int suits = 4;
        var suit = Enum.GetValues<Card>();
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