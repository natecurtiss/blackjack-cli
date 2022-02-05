using static System.Char;

namespace Blackjack;

public sealed class Card
{
    public readonly int Value;
    readonly string _name;
    bool _isHidden;

    public Card(string name, int value, bool isHidden = false)
    {
        _name = name;
        Value = value;
        _isHidden = isHidden;
    }
    
    public override string ToString()
    {
        if (_isHidden)
            return "a face down card";
        var startsWithVowel = "aeiou".Contains(ToLower(_name[0]));
        if (startsWithVowel)
            return $"an {_name}";
        return $"a {_name}";
    }

    public Card FaceDown()
    {
        Hide();
        return this;
    }
    public void Show() => _isHidden = false;
    public void Hide() => _isHidden = true;
}