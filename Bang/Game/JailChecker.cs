using Bang.PlayingCards;

namespace Bang.Game
{
    public class JailChecker : DrawChecker
    {
        protected override bool ShouldApplyEffect(BangGameCard card) => card.Suite == Suite.Hearts;
    }
}