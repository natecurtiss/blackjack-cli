namespace Blackjack;

public sealed class You : Player
{
    public int Chips { get; private set; }
    protected override string Name => "You";
    protected override string PresentTensePossession => "have";

    public You(int startingChips) => Chips = startingChips;

    public void Bet(int amount) => Chips -= amount;

    public void Reward(int amount) => Chips += amount;
}