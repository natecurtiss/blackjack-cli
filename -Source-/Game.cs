namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly Player _player = new();
    readonly Player _dealer = new();

    public void Start()
    {
        Shuffle();
        Shuffle();
        Shuffle();
        Deal();
        ShowPlayerHand();
        ShowDealerHand();
    }

    public void Shuffle() => _deck.Shuffle();

    public void Deal()
    {
        _player.Give(_deck.Top());
        _dealer.Give(_deck.Top());
        _player.Give(_deck.Top());
        _dealer.Give(_deck.Top());
    }

    public void ShowPlayerHand() => Console.WriteLine($"You have {_player.Cards()}");
    public void ShowDealerHand() => Console.WriteLine($"Dealer has {_dealer.Cards()}");
}