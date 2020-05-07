using System;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal abstract class HandlerState
    {
        public virtual bool IsFinalState => false;
        public virtual bool IsError => false;

        /// <summary>
        /// Handles card played by player on it's turn
        /// </summary>
        /// <param name="player"></param>
        /// <param name="card"></param>
        /// <param name="gameplay"></param>
        /// <returns></returns>
        public abstract HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay);
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Generates for the final states</exception>
        public abstract HandlerState ApplyReplyAction(BangGameCard card);
    }
}