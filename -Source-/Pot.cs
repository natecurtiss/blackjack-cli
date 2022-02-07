namespace Blackjack;

public sealed class Pot
{
    int _chips;
    
    public void Add(int amount) => _chips += amount;

    public int Take()
    {
        var chips = _chips;
        _chips = 0;
        return chips;
    }

    public void Empty() => _chips = 0;
}