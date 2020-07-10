using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class DynamiteActionHandler : CardActionHandler
    {
        public DynamiteActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            Logger.Info($"Player {gameplay.PlayerTurn.Character.Name} played dynamite card");
            gameplay.PlayerTurn.PlayerTablet.PutCard(card);

            return new DoneState(state);
        }
    }
}
