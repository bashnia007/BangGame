using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.GameEvents.CardEffects
{
    internal class JailActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            if (victim.Role == new Sheriff())
            {
                Logger.Info("Can't play Jail card on Sheriff!");
                return new ErrorState();
            }

            Logger.Info($"Player {gameplay.PlayerTurn.Character.Name} player Jail card on {victim.Character.Name}");
            victim.PlayerTablet.PutCard(card);

            return new DoneState();
        }
    }
}
