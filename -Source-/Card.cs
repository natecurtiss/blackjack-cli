using static System.Char;

namespace Blackjack;

public sealed class Card
{
    public readonly int PrimaryValue;
    public readonly int SecondaryValue;
    readonly string _name;
    bool _isHidden;

    public Card(string name, int value)
    {
        _name = name;
        PrimaryValue = value;
        SecondaryValue = value;
    }

    public Card(string name, int primaryValue, int secondaryValue)
    {
        _name = name;
        PrimaryValue = primaryValue;
        SecondaryValue = secondaryValue;
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
    
    public Card Clone() => new(_name, PrimaryValue, SecondaryValue);

    public Card FaceDown()
    {
        Hide();
        return this;
    }
    
    public void Show() => _isHidden = false;
    
    void Hide() => _isHidden = true;
}