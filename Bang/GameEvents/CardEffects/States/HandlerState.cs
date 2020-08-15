using System;
using Bang.Game;
using Bang.GameEvents.Enums;
using Bang.Players;
using Bang.PlayingCards;
using NLog;

namespace Bang.GameEvents.CardEffects.States
{
    internal abstract class HandlerState
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public virtual bool IsFinalState => false;
        public virtual bool IsError => false;

        protected internal bool BangAlreadyPlayed;

        protected Gameplay gameplay;
        protected HandlerState(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            BangAlreadyPlayed = false;
        }

        protected HandlerState(HandlerState previous) : this(previous.gameplay)
        {
            BangAlreadyPlayed = previous.BangAlreadyPlayed;
        }

        public virtual Response SideEffect { get; set; } = new Done();

        
        public abstract HandlerState ApplyCardEffect(Player player, BangGameCard card);

        public virtual HandlerState ApplyCardEffect(Player player) => throw new InvalidOperationException();
        public virtual HandlerState ApplyCardEffect(Player player, BangGameCard firstCard, BangGameCard secondCard) => throw new InvalidOperationException();
        public virtual HandlerState ApplyDrawOption(Player player, DrawOptions drawOption) => throw new InvalidOperationException();
    }
}