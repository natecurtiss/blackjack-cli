﻿using static System.Char;

namespace Blackjack;

public sealed class Card
{
    public readonly int PrimaryValue;
    public readonly int SecondaryValue;
    readonly string _name;
    
    public bool IsHidden { get; private set; }

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
        if (IsHidden)
            return "a Face-Down card";
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
    
    public void Show() => IsHidden = false;
    
    void Hide() => IsHidden = true;
}