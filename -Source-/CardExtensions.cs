using static System.Char;

namespace Blackjack;

public static class CardExtensions
{
    public static string Name(this Card card)
    {
        var name = Enum.GetName(card);
        if (name == null) return "Nothing";
        var startsWithVowel = "aeiou".Contains(ToLower(name[0]));
        if (startsWithVowel)
            return $"an {name}";
        return $"a {name}";
    }
}