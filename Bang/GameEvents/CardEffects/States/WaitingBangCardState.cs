using Bang.Players;
using Bang.PlayingCards;
using System;
using Bang.Game;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingBangCardState : HandlerState
    {
        private readonly Player victim;
        private readonly Player hitter;
        public WaitingBangCardState(Player victim, Player hitter)
        {
            this.victim = victim ?? throw new ArgumentNullException(nameof(victim));
            this.hitter = hitter ?? throw new ArgumentNullException(nameof(hitter));
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            if (card == null)
            {
                victim.LoseLifePoint(hitter, 1);

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
