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
        private readonly Player defender;
        private readonly Player opponent;

        private readonly DefenceStrategy defenceStrategy;

        internal WaitingBangAfterDuelState(Player defender, Player opponent, HandlerState previousState)
            : base(previousState)
        {
            this.defender = defender;
            this.opponent = opponent;
            this.defenceStrategy = new DefenceAgainstDuelStrategy(gameplay.PlayerTurn);
        }
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard firstCard)
        {
            if (player != defender)
                return new ErrorState(this);

            bool duelEnded = !defenceStrategy.Apply(player, firstCard);

            if (duelEnded)
            {
                Logger.Info($"Player {player.Name} lost duel");
                return new DoneState(this);
            }
            
            DefenceAgainstDuel response = new DefenceAgainstDuel {Player = opponent};
            
            return new WaitingBangAfterDuelState(opponent, defender, this){SideEffect = response};
        }
    }
}