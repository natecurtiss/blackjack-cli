namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly You _you;
    readonly Dealer _dealer = new();
    readonly Pot _pot = new();
    Card _dealerSecondCard = new("null", 0);

    public Game(int startingBalance)
    {
        _you = new(startingBalance);
        Console.Title = "Blackjack";
    }

    public void Start()
    {
        Console.Clear();
        _deck.Reset();
        _you.Reset();
        _dealer.Reset();

        Console.WriteLine($"How much would you like to bet? (Current balance is {_you.Chips})");
        while (true)
            if (!int.TryParse(Console.ReadLine(), out var amount))
            {
                Console.WriteLine($"Invalid amount - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else if (amount <= 0)
            {
                Console.WriteLine($"You have to bet something if you want to play - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else if (amount > _you.Chips)
            {
                Console.WriteLine($"You can't bet more than you have - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else
            {
                _pot.Add(amount);
                _you.Bet(amount, out var _, out var _);
                Console.WriteLine($"You bet {amount} (current balance is now {_you.Chips})");
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
            if (_dealer.CardsTotal() >= 17)
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
                    if (_you.CardsTotal() > _dealer.CardsTotal())
                        Win();
                    else if (_you.CardsTotal() < _dealer.CardsTotal())
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
        Console.WriteLine($"You {possession} {_you.CardNames()} ({_you.CardsTotal()})");
    }
    void ShowDealerHand(bool pastTense, bool showMissing)
    {
        var possession = pastTense ? "had" : "has";
        var total = showMissing ? _dealer.CardsTotal().ToString() : $"{_dealer.CardsTotal() - _dealerSecondCard.PrimaryValue} + ?";
        Console.WriteLine($"Dealer {possession} {_dealer.CardNames()} ({total})");
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
        _you.Reward((int) (_pot.Take() * 1.5f));
        Console.WriteLine($"\nYou won! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Win()
    {
        _you.Reward(_pot.Take() * 2);
        Console.WriteLine($"\nYou won! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Lose()
    {
        Console.WriteLine($"\nYou lost! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        if (_you.Chips <= 0)
            Console.WriteLine("\nYou're out of cash. GAME OVER.");
        else
            End();
    }

    void Tie()
    {
        _you.Reward(_pot.Take());
        Console.WriteLine($"\nIt's a tie! You now have {_you.Chips} chips");
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