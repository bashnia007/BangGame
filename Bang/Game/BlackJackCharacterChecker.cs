using Bang.PlayingCards;

namespace Bang.Game
{
    class BlackJackCharacterChecker : DrawChecker
    {
        protected override bool ShouldApplyEffect(BangGameCard card) 
            => card.Suite == Suite.Hearts || card.Suite == Suite.Diamonds;
    }
}
