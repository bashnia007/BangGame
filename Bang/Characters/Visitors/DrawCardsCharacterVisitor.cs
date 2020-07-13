using Bang.Game;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects.States;
using Bang.GameEvents.CharacterEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;

namespace Bang.Characters.Visitors
{
    internal class DrawCardsCharacterVisitor : ICharacterVisitor<Func<Gameplay, Player, HandlerState>>
    {
        public Func<Gameplay, Player, HandlerState> DefaultValue 
            => (gameplay, player)
            => ProvideTwoCards(gameplay, player);

        public Func<Gameplay, Player, HandlerState> Visit(BlackJack character)
        {
            return (gameplay, player) =>
            {
                var blackJackChecker = new BlackJackCharacterChecker();

                player.AddCardToHand(gameplay.DealCard());
                if (blackJackChecker.Peek(gameplay))
                {
                    player.AddCardToHand(gameplay.DealCard());
                }
                player.AddCardToHand(gameplay.DealCard());
                return new DoneState(gameplay);
            };
        }

        public Func<Gameplay, Player, HandlerState> Visit(KitCarlson character)
        {
            return (gameplay, player) =>
            {
                var list = new List<BangGameCard>();
                for (int i = 0; i < 3; i++)
                {
                    var newCard = gameplay.DealCard();
                    player.AddCardToHand(newCard);
                    list.Add(newCard);
                }
                var state = new WaitingForCardSelectionState(gameplay)
                {
                    SideEffect = new ChooseCardsResponse
                    {
                        CardsToChoose = list
                    }
                };
                return state;
            };
        }

        private HandlerState ProvideTwoCards(Gameplay gameplay, Player player)
        {
            for (int i = 0; i < 2; i++)
            {
                player.AddCardToHand(gameplay.DealCard());
            }
            return new DoneState(gameplay);
        }
    }
}
