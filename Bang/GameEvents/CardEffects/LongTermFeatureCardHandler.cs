using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class LongTermFeatureCardHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            if (victim.PlayerTablet.CanPutCard(card))
            {
                victim.PlayerTablet.PutCard(card);
                return new DoneState();
            }

            return new ErrorState();
        }
    }
}