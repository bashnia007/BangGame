using Bang.PlayingCards;

namespace Bang.Game
{
    public class BarrelChecker : DrawChecker
    {
        protected override bool ShouldApplyEffect(BangGameCard card) => card.Suite == Suite.Hearts;
    }
}