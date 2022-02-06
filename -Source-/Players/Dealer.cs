using static Blackjack.Rules;

namespace Blackjack;

public sealed class Dealer : Player
{
    public override void Give(Card card, out bool didWin, out bool didLose)
    {
        base.Give(card, out didWin, out didLose);
        if (_hand.Count >= STARTING_CARDS)
            _hand[FACE_DOWN_CARD].Show();
    }
}