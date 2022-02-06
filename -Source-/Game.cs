namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly Player _you = new();
    readonly Player _dealer = new();
    Card _dealerSecondCard = new("null", 0);
    int _balance;
    int _pot;

    public Game(int startingBalance)
    {
        _balance = startingBalance;
        Console.Title = "Blackjack";
    }

    public void Start()
    {
        Console.Clear();
        _deck.Reset();
        _you.Reset();
        _dealer.Reset();
        _pot = 0;

        Console.WriteLine($"How much would you like to bet? (Current balance is {_balance})");
        while (true)
            if (!int.TryParse(Console.ReadLine(), out var amount))
            {
                Console.WriteLine($"Invalid amount - How much would you like to bet? (Current balance is {_balance})");
            }
            else if (amount <= 0)
            {
                Console.WriteLine($"You have to bet something if you want to play - How much would you like to bet? (Current balance is {_balance})");
            }
            else if (amount > _balance)
            {
                Console.WriteLine($"You can't bet more than you have - How much would you like to bet? (Current balance is {_balance})");
            }
            else
            {
                _pot += amount;
                _balance -= amount;
                Console.WriteLine($"You bet {amount} (current balance is now {_balance})");
                break;
            }

        Shuffle();
        Shuffle();
        Shuffle();
        Deal(out var didPlayerWin, out var didDealerWin, out _dealerSecondCard);
        if (didPlayerWin && didDealerWin)
        {
            Tie();
        }
        else if (didPlayerWin)
        {
            Naturals();
        }
        else if (didDealerWin)
        {
            _dealerSecondCard.Show();
            Lose();
        }
        else
        {
            Console.WriteLine('\n');
            ShowYourHand(false);
            ShowDealerHand(false, false);
            PlayerTurn();
        }
    }

    void Shuffle() => _deck.Shuffle();

    void Deal(out bool didPlayerWin, out bool didDealerWin, out Card dealerSecondCard)
    {
        _you.Give(_deck.Top());
        _dealer.Give(_deck.Top());
        _you.Give(_deck.Top(), out didPlayerWin);
        dealerSecondCard = _deck.Top().FaceDown();
        _dealer.Give(dealerSecondCard, out didDealerWin);
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
                ShowYourHand(false);
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
        _dealerSecondCard.Show();
        ShowDealerHand(false, true);
        while (true)
        {
            DealerHit(out var didWin, out var didLose);
            if (_dealer.Total() >= 17)
            {
                if (didWin)
                {
                    Lose();
                }
                else if (didLose)
                {
                    Win();
                }
                else
                {
                    if (_you.Total() > _dealer.Total())
                        Win();
                    else if (_you.Total() < _dealer.Total())
                        Lose();
                    else
                        Tie();
                }
                break;
            }

        }
    }
    
    void ShowYourHand(bool pastTense)
    {
        var possession = pastTense ? "had" : "have";
        Console.WriteLine($"You {possession} {_you.Cards()} ({_you.Total()})");
    }
    void ShowDealerHand(bool pastTense, bool showMissing)
    {
        var possession = pastTense ? "had" : "has";
        var total = showMissing ? _dealer.Total().ToString() : $"{_dealer.Total() - _dealerSecondCard.PrimaryValue} + ?";
        Console.WriteLine($"Dealer {possession} {_dealer.Cards()} ({total})");
    }

    void Hit(out bool didWin, out bool didLose)
    {
        var card = _deck.Top();
        _you.Give(card, out didWin, out didLose);
        Console.WriteLine($"You drew {card}");
    }

    void DealerHit(out bool didWin, out bool didLose)
    {
        var card = _deck.Top();
        Console.WriteLine($"Dealer drew {card}");
        _dealer.Give(card, out didWin, out didLose);
    }

    void Naturals()
    {
        _balance += (int) (_pot * 1.5f);
        Console.WriteLine($"\nYou won! You now have {_balance} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Win()
    {
        _balance += _pot * 2;
        Console.WriteLine($"\nYou won! You now have {_balance} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Lose()
    {
        Console.WriteLine($"\nYou lost! You now have {_balance} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        if (_balance <= 0)
            Console.WriteLine("\nYou're out of cash. GAME OVER.");
        else
            End();
    }

    void Tie()
    {
        _balance += _pot;
        Console.WriteLine($"\nIt's a tie! You now have {_balance} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void End()
    {
        Console.WriteLine("\nPlay again? (Y/N)");
        while (true)
        {
            var response = Console.ReadLine()?.ToLower();
            if (response == "y")
                Start();
            else if (response == "n")
                break;
            else
                Console.WriteLine("Invalid response - Play again? (Y/N)");
        }
    }
}