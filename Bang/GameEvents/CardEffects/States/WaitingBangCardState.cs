using Bang.Players;
using Bang.PlayingCards;
using System;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingBangCardState : HandlerState
    {
        private readonly Player victim;
        public WaitingBangCardState(Player victim)
        {
            this.victim = victim;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            if (card == null)
            {
                victim.LoseLifePoint();
                // TODO Future: check if victim alive
                return new DoneState();
            }
            else if (card == new BangCardType())
            {
                victim.DropCard(card);
                return new DoneState();
            }
            else
            {
                throw new ArgumentException($"Wrong card. Expected {new BangCardType().Description} or null");
            }
        }
    }
}
