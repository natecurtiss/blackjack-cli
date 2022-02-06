namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly You _you;
    readonly Dealer _dealer = new();
    readonly Pot _pot = new();

    readonly Action _start;
    readonly Action<string> _say;
    readonly Func<string> _getInput;

    Card _dealerSecondCard = new("null", 0);

    public Game(int startingBalance, Action onStart, Action<string> onSay, Func<string> onInput)
    {
        _you = new(startingBalance);
        _start = onStart;
        _say = onSay;
        _getInput = onInput;
    }

    public void Start()
    {
        _start();
        _deck.Reset();
        _you.Reset();
        _dealer.Reset();

        _say($"How much would you like to bet? (Current balance is {_you.Chips})");
        while (true)
            if (!int.TryParse(_getInput(), out var amount))
            {
                _say($"Invalid amount - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else if (amount <= 0)
            {
                _say($"You have to bet something if you want to play - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else if (amount > _you.Chips)
            {
                _say($"You can't bet more than you have - How much would you like to bet? (Current balance is {_you.Chips})");
            }
            else
            {
                _pot.Add(amount);
                _you.Bet(amount, out var _, out var _);
                _say($"You bet {amount} (current balance is now {_you.Chips})");
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
            _say("\n");
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
            _say("\n");

            _say("Please enter a command (hit, stand, or hand).");
            var command = _getInput().ToLower();
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
                _say("That's not a command, please enter a valid one.");
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
        _say($"You {possession} {_you.CardNames()} ({_you.CardsTotal()})");
    }
    void ShowDealerHand(bool pastTense, bool showMissing)
    {
        var possession = pastTense ? "had" : "has";
        var total = showMissing ? _dealer.CardsTotal().ToString() : $"{_dealer.CardsTotal() - _dealerSecondCard.PrimaryValue} + ?";
        _say($"Dealer {possession} {_dealer.CardNames()} ({total})");
    }

    void Hit(out bool didWin, out bool didLose)
    {
        var card = _deck.Top();
        _you.Give(card, out didWin, out didLose);
        _say($"You drew {card}");
    }

    void DealerHit(out bool didWin, out bool didLose)
    {
        var card = _deck.Top();
        _say($"Dealer drew {card}");
        _dealer.Give(card, out didWin, out didLose);
    }

    void Naturals()
    {
        _you.Reward((int) (_pot.Take() * 1.5f));
        _say($"\nYou won! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Win()
    {
        _you.Reward(_pot.Take() * 2);
        _say($"\nYou won! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void Lose()
    {
        _say($"\nYou lost! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        if (_you.Chips <= 0)
            _say("\nYou're out of cash. GAME OVER.");
        else
            End();
    }

    void Tie()
    {
        _you.Reward(_pot.Take());
        _say($"\nIt's a tie! You now have {_you.Chips} chips");
        ShowYourHand(true);
        ShowDealerHand(true, true);
        End();
    }

    void End()
    {
        _say("\nPlay again? (Y/N)");
        while (true)
        {
            var response = _getInput().ToLower();
            if (response == "y")
            {
                Start();
                break;
            }
            if (response == "n")
                break;
            _say("Invalid response - Play again? (Y/N)");
        }
    }
}