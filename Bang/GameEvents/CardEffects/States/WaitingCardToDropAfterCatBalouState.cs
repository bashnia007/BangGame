using System;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToDropAfterCatBalouState : HandlerState
    {
        private Player victim;
        private Game.Gameplay gamePlay;

        internal WaitingCardToDropAfterCatBalouState(Player victim, Game.Gameplay gameplay)
        {
            this.victim = victim;
            this.gamePlay = gameplay;
        }
            
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new System.NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            victim.DropActiveCard(card);
            return new DoneState();
        }

        public override HandlerState ApplyReplyAction()
        {
            var card = RandomCardChooser.ChooseCard(victim.Hand);
            victim.DropCard(card);
            
            return new DoneState();
        }
    }
}