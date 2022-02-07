using static Blackjack.Rules;

namespace Blackjack;

public sealed class Dealer : Player
{
    protected override string Name => "Dealer";
    protected override string PresentTensePossession => "has";

    public void ShowAllCards() => _hand[DEALER_FACE_DOWN_CARD].Show();

    protected override string VisibleCardsTotal()
    {
        var total = CardsTotal();
        if (_hand[DEALER_FACE_DOWN_CARD].IsHidden)
        {
            total -= _hand[DEALER_FACE_DOWN_CARD].PrimaryValue;
            return $"{total} + ?";
        }
        return total.ToString();
    }
}