using System;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal class WaitingBangAfterDuelState : HandlerState
    {
        protected override CardType ExpectedCard => new BangCardType();

        private readonly Player defender;
        private readonly Player opponent;
        private readonly Game.Gameplay gamePlay;
        private readonly DefenceStrategy defenceStrategy;

        internal WaitingBangAfterDuelState(Game.Gameplay gameplay, Player defender, Player opponent)
        {
            this.gamePlay = gameplay;
            this.defender = defender;
            this.opponent = opponent;
            this.defenceStrategy = new DefenceAgainstDuelStrategy(gameplay.PlayerTurn);
        }
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay) => 
            throw new NotImplementedException();
        
        public override HandlerState ApplyReplyAction(Player player, BangGameCard firstCard)
        {
            if (player != defender)
                return new ErrorState();

            bool duelEnded = !defenceStrategy.Apply(player, firstCard);

            if (duelEnded)
            {
                Logger.Info($"Player {player.Name} lost duel");
                return new DoneState();
            }
            
            DefenceAgainstDuel response = new DefenceAgainstDuel {Player = opponent};
            
            return new WaitingBangAfterDuelState(gamePlay, opponent, defender){SideEffect = response};
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new System.NotImplementedException();
        }
    }
}