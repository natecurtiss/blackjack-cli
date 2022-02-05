using static Blackjack.Role;

namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly Player _you = new(You);
    readonly Player _dealer = new(Dealer);

    public void Start()
    {
        Shuffle();
        Shuffle();
        Shuffle();
        Deal();
        ShowYourHand();
        ShowDealerHand();
    }

    public void Shuffle() => _deck.Shuffle();

    public void Deal()
    {
        _you.Give(_deck.Top());
        _dealer.Give(_deck.Top());
        _you.Give(_deck.Top());
        _dealer.Give(_deck.Top());
    }

    public void ShowYourHand() => Console.WriteLine($"You have {_you.Cards()}");
    public void ShowDealerHand() => Console.WriteLine($"Dealer has {_dealer.Cards()}");
}