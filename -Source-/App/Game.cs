namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly Player _you = new();
    readonly Player _dealer = new();

    public void Start()
    {
        Shuffle();
        Shuffle();
        Shuffle();
        Deal(out var didPlayerWin, out var didDealerWin);
        if (didPlayerWin)
        {
            // TODO: Player wins off deal.
        }
        else if (didDealerWin)
        {
            // TODO: Dealer wins off deal.
        }
        ShowYourHand();
        ShowDealerHand();
        PlayerTurn();
    }

    void Shuffle() => _deck.Shuffle();

    void Deal(out bool didPlayerWin, out bool didDealerWin)
    {
        _you.Give(_deck.Top());
        _dealer.Give(_deck.Top());
        _you.Give(_deck.Top(), out didPlayerWin);
        _dealer.Give(_deck.Top().FaceDown(), out didDealerWin);
    }

    void PlayerTurn()
    {
        while (true)
        {
            Console.WriteLine('\n');

            Console.WriteLine("Please enter a command (hit, stand, or hand).");
            var command = Console.ReadLine()?.ToLower();
            if (command == "hand")
            {
                ShowYourHand();
            }
            else if (command == "hit")
            {
                Hit(out var didWin, out var didLose);
                if (didWin)
                {
                    Win();
                    break;
                }
                if (didLose)
                {
                    Lose();
                    break;
                }
            }
            else if (command == "stand")
            {
                DealerTurn();
                break;
            }
            else
            {
                Console.WriteLine("That's not a command, please enter a valid one.");
            }
        }
    }

    void DealerTurn()
    {
        while (true)
        {
            // TODO: Make Dealer draw cards.
        }
    }
    
    void ShowYourHand() => Console.WriteLine($"You have {_you.Cards()} ({_you.Total()})");
    void ShowDealerHand() => Console.WriteLine($"Dealer has {_dealer.Cards()} ({_dealer.Total()})");

    void Hit(out bool didWin, out bool didLose)
    {
        var card = _deck.Top();
        _you.Give(card, out didWin, out didLose);
        Console.WriteLine($"You drew {card}");
    }

    void Win()
    {
        // TODO: Player win.
        Console.WriteLine("\n You win!");
    }

    void Lose()
    {
        // TODO: Player lose.
        Console.WriteLine("\n You lose!");
    }
}