using System.Text;
using static Blackjack.Role;

namespace Blackjack;

public sealed class Player
{
    readonly List<Card> _hand = new();
    readonly bool _isDealer;
    readonly string _faceDown = "a face-down card";

    public Player(Role type) => _isDealer = type == Dealer;

    public int Total() => _hand.Sum(card => (int) card);
    public string Cards()
    {
        var cards = _hand.Count;
        var last = cards - 1;
        if (cards == 0) 
            return "Nothing";
        if (cards == 1)
        {
            if (_isDealer) 
                return $"{_faceDown}.";
            return $"{_hand[0].Name()}.";
        }
        if (cards == 2)
        {
            if (_isDealer) 
                return $"{_faceDown} and {_hand[1].Name()}";
            return $"{_hand[0].Name()} and {_hand[1].Name()}";
        }
        var sb = new StringBuilder();
        for (var i = 0; i < cards; i++)
        {
            if (i == 0 && _isDealer)
                sb.Append($"{_faceDown}, ");
            else if (i == last)
                sb.Append($"and {_hand[i].Name()}.");
            else
                sb.Append($"{_hand[i].Name()}, ");
        }
        return sb.ToString();
    }
    
    public void Give(Card card) => _hand.Add(card);
}