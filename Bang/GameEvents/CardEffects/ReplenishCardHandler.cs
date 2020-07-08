using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal abstract class ReplenishCardHandler : CardActionHandler
    {
        protected ReplenishCardHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        protected abstract short CardsToTakeAmount { get; }

        public override HandlerState ApplyEffect(Player player, BangGameCard card)
        {
            if (gameplay.PlayerTurn != player) return new ErrorState(state);
            
            for (int i = 0; i < CardsToTakeAmount; i++)
            {
                var newCard = gameplay.DealCard();
                player.AddCardToHand(newCard);
            }

            return new DoneState(state);
        }
    }
}