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
            if (!((ChooseCardsResponse)SideEffect).CardsToChoose.Contains(card))
            {
                Logger.Error("Kit Karlson can't drop card from his hand during phase one");
                return this;
            }

            player.PutCardOnDeck(card);
            return new DoneState(gameplay);
        }
    }
}
