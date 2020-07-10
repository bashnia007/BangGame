using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class LongTermFeatureCardHandler : CardActionHandler
    {
        public LongTermFeatureCardHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            if (victim.PlayerTablet.CanPutCard(card))
            {
                victim.PlayerTablet.PutCard(card);
                return new DoneState(state);
            }

            return new ErrorState(state) {SideEffect = new NotAllowedOperation()};
        }
    }
}