using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal class DuelActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            Logger.Info($"Player {gameplay.PlayerTurn.Name} challenged {victim.Name} to a duel");
            DefenceAgainstDuel response = new DefenceAgainstDuel {Player = victim};
            
            return new WaitingBangAfterDuelState(gameplay, victim, gameplay.PlayerTurn){SideEffect = response};
        }
    }
}