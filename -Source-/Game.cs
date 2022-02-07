using static Blackjack.Rules;

namespace Blackjack;

public sealed class Game
{
    readonly Deck _deck = new();
    readonly You _you;
    readonly Dealer _dealer = new();
    readonly Pot _pot = new();

    readonly Action _start;
    readonly Action<string> _say;
    readonly Func<string> _getStringInput;
    readonly Func<string, int> _getIntegerInput;

    public Game(int startingBalance, Action onStart, Action<string> onSay, Func<string> onStringInput, Func<string, int> onIntegerInput)
    {
        _you = new(startingBalance);
        _start = onStart;
        _say = onSay;
        _getStringInput = onStringInput;
        _getIntegerInput = onIntegerInput;
    }

    public void Start()
    {
        _start();
        _deck.Reset();
        _you.Reset();
        _dealer.Reset();

        BettingRound();
        Shuffle();
        if (Deal() == TurnResult.Continue)
        {
            _say(_you.Hand(Tense.Present));
            _say(_dealer.Hand(Tense.Present));
            if (PlayerTurn() == TurnResult.Continue)
                DealerTurn();
        }
    }

    void BettingRound()
    {
        var amount = 0;
        var chips = _you.Chips;
        _say($"How much would you like to bet? (Current balance is {_you.Chips})");
        while (amount <= 0 || amount > chips)
        {
            amount = _getIntegerInput($"Invalid amount - How much would you like to bet? (Current balance is {_you.Chips})");
            if (amount <= 0)
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
                _you.Bet(amount);
                _say($"You bet {amount}. (Current balance is now {_you.Chips})\n");
            }
        }
    }

    void Shuffle()
    {
        for (var i = 0; i < Rules.SHUFFLES; i++)
            _deck.Shuffle();
    }

    TurnResult Deal()
    {
        var didPlayerWin = false;
        var didDealerWin = false;
        for (var i = 0; i < STARTING_CARDS; i++)
            if (i == DEALER_FACE_DOWN_CARD)
            {
                _you.Give(_deck.Draw(), out didPlayerWin);
                _dealer.Give(_deck.Draw().FaceDown(), out didDealerWin);
            }
            else
            {
                _you.Give(_deck.Draw());
                _dealer.Give(_deck.Draw());
            }
        if (didPlayerWin && didDealerWin)
        {
            NaturalsTie();
            return TurnResult.Tie;
        }
        if (didPlayerWin)
        {
            NaturalsWin();
            return TurnResult.Win;
        }
        if (didDealerWin)
        {
            NaturalsLose();
            return TurnResult.Lose;
        }
        return TurnResult.Continue;
    }

    TurnResult PlayerTurn()
    {
        while (true)
        {
            _say("\nPlease enter a command (hit, stand, or hand).");
            var command = _getStringInput().ToLower();
            if (command == "hand")
            {
                _say(_you.Hand(Tense.Present));
            }
            else if (command == "hit")
            {
                var result = Hit();
                if (result != TurnResult.Continue)
                    return result;
            }
            else if (command == "stand")
            {
                return TurnResult.Continue;
            }
            else
            {
                _say("That's not a command, please enter a valid one.");
            }
        }
    }

    void DealerTurn()
    {
        _dealer.ShowAllCards();
        _say(_dealer.Hand(Tense.Present));
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
                return;
            }
        }
    }

    TurnResult Hit()
    {
        var card = _deck.Draw();
        _you.Give(card, out var didWin, out var didLose);
        _say($"You drew {card}");
        if (didWin)
        {
            Win();
            return TurnResult.Win;
        }
        if (didLose)
        {
            Lose();
            return TurnResult.Lose;
        }
        return TurnResult.Continue;
    }

    void DealerHit(out bool didWin, out bool didLose)
    {
        var card = _deck.Draw();
        _say($"Dealer drew {card}");
        _dealer.Give(card, out didWin, out didLose);
    }

    void NaturalsWin()
    {
        _you.Reward((int) (_pot.Take() * 1.5f));
        _say($"You won with naturals! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        EndRound();
    }

    void NaturalsLose()
    {
        _pot.Empty();
        _say($"You lost to a Blackjack! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        if (_you.Chips <= 0)
            _say("\nYou're out of cash. GAME OVER.");
        else
            EndRound();
    }

    void NaturalsTie()
    {
        _you.Reward(_pot.Take());
        _say($"You and the dealer both tied with a Blackjack! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        EndRound();
    }

    void Win()
    {
        _you.Reward(_pot.Take() * 2);
        _say($"\nYou won! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        EndRound();
    }

    void Lose()
    {
        _pot.Empty();
        _say($"\nYou lost! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        if (_you.Chips <= 0)
            _say("\nYou're out of cash. GAME OVER.");
        else
            EndRound();
    }

    void Tie()
    {
        _you.Reward(_pot.Take());
        _say($"\nIt's a tie! You now have {_you.Chips} chips");
        _dealer.ShowAllCards();
        _say(_you.Hand(Tense.Past));
        _say(_dealer.Hand(Tense.Past));
        EndRound();
    }

    void EndRound()
    {
        _say("\nPlay again? (Y/N)");
        while (true)
        {
            var response = _getStringInput().ToLower();
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