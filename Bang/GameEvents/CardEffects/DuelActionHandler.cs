using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal class DuelActionHandler : CardActionHandler
    {
        public DuelActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            Logger.Info($"Player {gameplay.PlayerTurn.Name} challenged {victim.Name} to a duel");
            DefenceAgainstDuel response = new DefenceAgainstDuel {Player = victim};
            
            return new WaitingBangAfterDuelState(victim, gameplay.PlayerTurn, state){SideEffect = response};
        }
    }
}