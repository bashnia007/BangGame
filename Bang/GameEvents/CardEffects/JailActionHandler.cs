using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.GameEvents.CardEffects
{
    internal class JailActionHandler : CardActionHandler
    {
        public JailActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            if (victim.Role == new Sheriff())
            {
                Logger.Info("Can't play Jail card on Sheriff!");
                return new ErrorState(state);
            }

            Logger.Info($"Player {gameplay.PlayerTurn.Character.Name} played Jail card on {victim.Character.Name}");
            victim.PlayerTablet.PutCard(card);

            return new DoneState(state)
            {
                SideEffect = new LeaveCardOnTheTableResponse()
            };
        }
    }
}
