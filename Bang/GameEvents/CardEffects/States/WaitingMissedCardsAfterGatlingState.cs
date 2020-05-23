using Bang.Characters;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingMissedCardsAfterGatlingState : HandlerState
    {
        private Dictionary<Player, HandlerState> victimStates;
        private Game.Gameplay gameplay;
        public WaitingMissedCardsAfterGatlingState(Dictionary<Player, HandlerState> victimStates)
        {
            this.victimStates = victimStates;
        }
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            if (card == null || gameplay.PlayerTurn.Character is SlabTheKiller)
            {
                //victim.LoseLifePoint();
                // TODO Future: check if victim alive
                return new DoneState();
            }
            else if (card == new MissedCardType())
            {
                //victim.DropCard(card);
                return new DoneState();
            }
            else
            {
                throw new ArgumentException($"Wrong card. Expected {new MissedCardType().Description} or null");
            }
        }

        public override HandlerState ApplyReplyAction(BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ApplyReplyAction(firstCard);

            if (!(gameplay.PlayerTurn.Character is SlabTheKiller))
                throw new InvalidOperationException("Only Slab the Killer's Bang requires two Missed cards");

            var missedCard = new MissedCardType();
            if (firstCard == missedCard && secondCard == missedCard)
            {
                //victim.DropCard(firstCard);
                //victim.DropCard(secondCard);

                return new DoneState();
            }
            // TODO Check if victim is alive
            //victim.LoseLifePoint();

            return new DoneState();

            throw new NotImplementedException();
        }

        private HandlerState CheckAll()
        {
            if (victimStates.Count == 0)
            {
                return new DoneState();
            }
            return this;
        }
    }
}
