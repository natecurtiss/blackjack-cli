namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly Player _player = new();
    readonly Player _dealer = new();

    public void Start()
    {
        Shuffle();
        Deal();
        Console.WriteLine(_player.Cards());
        Console.WriteLine(_dealer.Cards());
    }

    void Shuffle()
    {
        for (var i = 0; i < 3; i++)
            _deck.Shuffle();
    }

    void Deal()
    {
        _player.Give(_deck.Top());
        _dealer.Give(_deck.Top());
        _player.Give(_deck.Top());
        _dealer.Give(_deck.Top());
    }
}