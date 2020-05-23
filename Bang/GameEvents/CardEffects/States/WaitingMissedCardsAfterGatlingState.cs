using Bang.Characters;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects.States
{
    public class WaitingMissedCardsAfterGatlingState : HandlerState
    {
        private readonly Dictionary<Player, HandlerState> victimStates;
        private readonly Game.Gameplay gameplay;

        private Player currentVictim;

        public WaitingMissedCardsAfterGatlingState(Dictionary<Player, HandlerState> victimStates, Game.Gameplay gameplay)
        {
            this.victimStates = victimStates;
            this.gameplay = gameplay;
        }
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            if (card == null || gameplay.PlayerTurn.Character is SlabTheKiller)
            {
                currentVictim.LoseLifePoint();
                // TODO Future: check if victim alive
                victimStates[currentVictim] = new DoneState();
                return UpdateStatus();
            }
            else if (card == new MissedCardType())
            {
                currentVictim.DropCard(card);
                victimStates[currentVictim] = new DoneState();
                return UpdateStatus();
            }
            else
            {
                throw new ArgumentException($"Wrong card. Expected {new MissedCardType().Description} or null");
            }
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard firstCard, BangGameCard secondCard)
        {
            currentVictim = victim;
            if (secondCard == null) return ApplyReplyAction(firstCard);

            if (!(gameplay.PlayerTurn.Character is SlabTheKiller))
                throw new InvalidOperationException("Only Slab the Killer's Bang requires two Missed cards");

            var missedCard = new MissedCardType();
            if (firstCard == missedCard && secondCard == missedCard)
            {
                victim.DropCard(firstCard);
                victim.DropCard(secondCard);

                victimStates[victim] = new DoneState();
                return UpdateStatus();
            }
            // TODO Check if victim is alive
            victim.LoseLifePoint();

            victimStates[victim] = new DoneState();
            return UpdateStatus();
        }

        private HandlerState UpdateStatus()
        {
            return victimStates.All(state => state.Value is DoneState) 
                ? new DoneState() 
                : (HandlerState)this;
        }
    }
}
