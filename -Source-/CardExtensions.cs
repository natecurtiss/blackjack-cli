namespace Blackjack;

public static class CardExtensions
{
    public static string Name(this Card card) => $"a {Enum.GetName(card)}";
}