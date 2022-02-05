using System.Text;

namespace Blackjack;

public sealed class Player
{
    readonly List<Card> _hand = new();

    public int Total() => _hand.Sum(card => card.Value);
    public string Cards()
    {
        var cards = _hand.Count;
        var last = cards - 1;
        switch (cards)
        {
            case 0: return "Nothing";
            case 1: return $"{_hand[0]}.";
            case 2: return $"{_hand[0]} and {_hand[1]}";
        }
        var sb = new StringBuilder();
        for (var i = 0; i < cards; i++)
            if (i != last) 
                sb.Append($"{_hand[i]}, ");
            else 
                sb.Append($"and {_hand[i]}.");
        return sb.ToString();
    }
    
    public void Give(Card card) => _hand.Add(card);
}