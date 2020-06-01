using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal abstract class CardActionHandler
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public abstract HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card);
    }
}