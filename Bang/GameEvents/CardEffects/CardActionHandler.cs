using System;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal abstract class CardActionHandler
    {
        public abstract HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card);
    }
}