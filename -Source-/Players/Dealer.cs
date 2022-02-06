using static Blackjack.Rules;

namespace Blackjack;

public sealed class Dealer : Player
{
    public void ShowAllCards() => _hand[DEALER_FACE_DOWN_CARD].Show();
}