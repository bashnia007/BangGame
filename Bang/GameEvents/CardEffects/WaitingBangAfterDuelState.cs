using System;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal class WaitingBangAfterDuelState : HandlerState
    {
        private readonly Player defender;
        private readonly Player opponent;
        private readonly Game.Gameplay gamePlay;

        internal WaitingBangAfterDuelState(Game.Gameplay gameplay, Player defender, Player opponent)
        {
            this.gamePlay = gameplay;
            this.defender = defender;
            this.opponent = opponent;
        }
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay) => 
            throw new NotImplementedException();
        
        public override HandlerState ApplyReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (player != defender)
                return new ErrorState();

            if (firstCard != new BangCardType())
            {
                Logger.Info($"Player {player.Name} lost duel");
                // FAQ.
                // Q26. If an Outlaw plays a Duel and loses, does the player who won the Duel draw the 3 cards reward?
                // A. No. The reward goes to the player who played the card which was responsible of the elimination of the Outlaw. But since that card was played by the Outlaw himself, no one can gain the reward.
                player.LoseLifePoint(gamePlay.PlayerTurn);
                
                return new DoneState();
            }
            
            player.LoseCard(firstCard);
            gamePlay.DropCard(firstCard);

            DefenceAgainstDuel response = new DefenceAgainstDuel {Player = opponent};
            
            return new WaitingBangAfterDuelState(gamePlay, opponent, defender){SideEffect = response};
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new System.NotImplementedException();
        }
    }
}