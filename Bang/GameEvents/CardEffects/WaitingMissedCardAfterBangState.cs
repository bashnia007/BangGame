using System;
using Bang.Characters;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class WaitingMissedCardAfterBangState : HandlerState
    {
        public override bool IsFinalState => false;
        public override bool IsError => false;

        private Player victim;
        private Game.Gameplay gameplay;
        
        public WaitingMissedCardAfterBangState(Player victim, Game.Gameplay gameplay)
        {
            this.victim = victim;
            this.gameplay = gameplay;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new InvalidOperationException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            if (card == null || gameplay.PlayerTurn.Character is SlabTheKiller)
            {
                victim.LoseLifePoint(gameplay.PlayerTurn, 1);
                // TODO Future: check if victim alive
                return new DoneState();
            }
            else if (card == new MissedCardType())
            {
                victim.DropCard(card);
                return new DoneState();
            }
            else
            {
                throw new ArgumentException($"Wrong card. Expected {new MissedCardType().Description} or null");
            }
            
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ApplyReplyAction(firstCard);
            
            if (!(gameplay.PlayerTurn.Character is SlabTheKiller))
                throw new InvalidOperationException("Only Slab the Killer's Bang requires two Missed cards");
            
            var missedCard = new MissedCardType();
            if (firstCard == missedCard && secondCard == missedCard)
            {
                victim.DropCard(firstCard);
                victim.DropCard(secondCard);
                
                return new DoneState();
            }
            // TODO Check if victim is alive
            victim.LoseLifePoint(gameplay.PlayerTurn, 1);
            
            return new DoneState();
        }
    }
}