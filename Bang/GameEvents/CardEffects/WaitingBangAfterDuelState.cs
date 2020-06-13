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
            if (player.Id != defender.Id)
                return new ErrorState();

            if (firstCard != new BangCardType())
            {
                Logger.Info($"Player {player.Name} lost duel");
                player.LoseLifePoint();
                
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