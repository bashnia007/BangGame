using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System;

namespace Bang.GameEvents.CharacterEffects.States
{
    internal class WaitingForCardSelectionState : HandlerState
    {
        public WaitingForCardSelectionState(Gameplay gameplay) : base(gameplay)
        { }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard card)
        {
            player.PutCardOnDeck(card);
            return new DoneState(gameplay);
        }
    }
}
