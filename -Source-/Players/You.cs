namespace Blackjack;

public sealed class You : Player
{
    public int Chips { get; private set; }

    public You(int startingChips) => Chips = startingChips;

    public void Bet(int amount, out bool isLegal, out string message)
    {
        if (amount <= 0 || amount > Chips)
        {
            isLegal = false;
        }
        else
        {
            isLegal = true;
            Chips -= amount;
        }
        message = amount <= 0 ? " " : amount > Chips ? " " : " ";
    }

    public void Reward(int amount) => Chips += amount;
}