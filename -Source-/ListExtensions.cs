namespace Blackjack;

static class ListExtensions
{
    static readonly Random _random = new();
    
    public static void Shuffle<T>(this IList<T> list)
    {
        var i = list.Count;
        while (i > 1)
        {
            i--;
            var k = _random.Next(i + 1);
            (list[k], list[i]) = (list[i], list[k]);
        }
    }
}