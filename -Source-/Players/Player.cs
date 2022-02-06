using System.Text;

namespace Blackjack;

public abstract class Player
{
    protected readonly List<Card> _hand = new();

    public void Reset() => _hand.Clear();

    public int CardsTotal()
    {
        var primaryTotal = _hand.Sum(card => card.PrimaryValue);
        var secondaryTotal = _hand.Sum(card => card.SecondaryValue);
        if (primaryTotal > 21)
            return secondaryTotal;
        return primaryTotal;
    }
    
    public string CardNames()
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

    public void Give(Card card) => Give(card, out var _, out var _);
    public void Give(Card card, out bool didWin) => Give(card, out didWin, out var _);
    public virtual void Give(Card card, out bool didWin, out bool didLose)
    {
        _hand.Add(card);
        didWin = CardsTotal() == 21;
        didLose = CardsTotal() > 21;
    }
    
}