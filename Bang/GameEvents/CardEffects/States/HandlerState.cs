using System;
using Bang.Game;
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
        
        /// <summary>
        /// Handles card played by player on it's turn
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <param name="gameplay"></param>
        /// <returns></returns>
        public abstract HandlerState ApplyCardEffect(Player player, BangGameCard card);

        public virtual HandlerState ApplyReplyAction(Player player) => throw new InvalidOperationException();
        public abstract HandlerState ApplyReplyAction(Player player, BangGameCard card);
        public virtual HandlerState ApplyReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard) => throw new InvalidOperationException();
    }
}