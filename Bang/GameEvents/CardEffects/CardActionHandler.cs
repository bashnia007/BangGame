using System;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects
{
    internal abstract class CardActionHandler
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected Gameplay gameplay;
        protected HandlerState state;
        protected CardActionHandler(Gameplay gameplay, HandlerState state)
        {
            this.gameplay = gameplay ?? throw new ArgumentNullException(nameof(gameplay));
            this.state = state ?? throw new ArgumentNullException(nameof(state));
        }
        
        public abstract HandlerState ApplyEffect(Player victim, BangGameCard card);
    }
}