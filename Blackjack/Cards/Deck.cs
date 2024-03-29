﻿using static Blackjack.Rules;

namespace Blackjack;

public sealed class Deck
{
    readonly List<Card> _cards = new();

    public void Reset()
    {
        _cards.Clear();
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
            new("Ace", 11, 1),
        };
        foreach (var card in suit)
            for (var i = 0; i < SUITS; i++)
                _cards.Add(card.Clone());
    }

    public void Shuffle() => _cards.Shuffle();

    public Card Draw()
    {
        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}