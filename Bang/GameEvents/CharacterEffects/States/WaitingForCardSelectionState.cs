using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;

namespace Bang.GameEvents.CharacterEffects.States
{
    internal class WaitingForCardSelectionState : HandlerState
    {
        private readonly List<BangGameCard> expectedCards;

        public WaitingForCardSelectionState(Gameplay gameplay, List<BangGameCard> cardsToShoose) : base(gameplay)
        {
            SideEffect = new ChooseCardsResponse
            {
                CardsToChoose = cardsToShoose
            };
            expectedCards = cardsToShoose;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            if (!expectedCards.Contains(card))
            {
                return new ErrorState(this, $"Unexpected card in state {GetType()} for player {player.Name}");
            }

            player.PutCardOnDeck(card);
            return new DoneState(gameplay);
        }
    }
}
