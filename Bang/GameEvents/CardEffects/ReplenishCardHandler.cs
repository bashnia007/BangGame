using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal abstract class ReplenishCardHandler : CardActionHandler
    {
        protected abstract short CardsToTakeAmount { get; }

        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player player, BangGameCard card)
        {
            if (gameplay.PlayerTurn != player) return new ErrorState();
            
            for (int i = 0; i < CardsToTakeAmount; i++)
            {
                var newCard = gameplay.DealCard();
                player.AddCardToHand(newCard);
            }

            return new DoneState();
        }
    }
}