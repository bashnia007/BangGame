using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class DynamiteActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            Logger.Info($"Player {gameplay.PlayerTurn.Character.Name} player dynamite card");
            gameplay.PlayerTurn.PlayerTablet.PutCard(card);

            return new DoneState();
        }
    }
}
