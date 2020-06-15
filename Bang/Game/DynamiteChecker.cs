using Bang.PlayingCards;

namespace Bang.Game
{
    public class DynamiteChecker : DrawChecker
    {
        protected override bool ShouldApplyEffect(BangGameCard card)
             => card.Suite == Suite.Clubs && card.Rank <= Rank.Nine;
    }
}