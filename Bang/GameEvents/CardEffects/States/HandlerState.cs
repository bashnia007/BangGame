using System;
using Bang.Characters.Visitors;
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
        protected virtual CardType ExpectedCard => null;

        public virtual Response SideEffect { get; set; } = new Done();
        
        /// <summary>
        /// Handles card played by player on it's turn
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <param name="gameplay"></param>
        /// <returns></returns>
        public abstract HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay);
        

        public abstract HandlerState ApplyReplyAction(Player player, BangGameCard card);

        public virtual HandlerState ApplyReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard) => throw new InvalidOperationException();
        public virtual HandlerState ApplyReplyAction() => throw new InvalidOperationException();

        protected virtual bool IsValidCard(Player player, BangGameCard card)
        {
            return player.Character.Accept(new CardValidationForCharacterVisitor()).Invoke(card, ExpectedCard);
        }
    }
}