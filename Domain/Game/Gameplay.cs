using Domain.Characters;
using Domain.Players;
using Domain.PlayingCards;
using Domain.Roles;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    [Serializable]
    public class Gameplay
    {
        private Deck<BangGameCard> deck;
        private Stack<BangGameCard> discardedCards;
        private List<Player> players;
        
        public void Initialize(List<Player> players)
        {
            discardedCards = new Stack<BangGameCard>();
            this.players = players;
            deck = new Deck<BangGameCard>(GameInitializer.PlayingCards);
            deck.Shuffle();

            ProvideCardsForPlayers();
            
            foreach (var player in this.players)
            {
                player.CardsDropped += DropCardsFromHand;
                player.CardsTaken += TakeCardsOnHand;
                player.PlayerTablet.CardDropped += DropCardsFromHand;
            }

            
        }

        public CardType GetTopCardFromDiscarded()
        {
            return discardedCards.Peek();
        }

        private void DropCardsFromHand(List<BangGameCard> cardsToDrop)
        {
            foreach (var card in cardsToDrop)
            {
                discardedCards.Push(card);
            }
        }

        private List<BangGameCard> TakeCardsOnHand(short amount, string playerId)
        {
            var result = new List<BangGameCard>();

            for (short i = 0; i < amount; i++)
            {
                if (deck.Count == 0)
                {
                    ResetDeck();
                }
                result.Add(deck.Dequeue());
            }

            return result;
        }

        private void ProvideCardsForPlayers()
        {
            var roles = new Deck<Role>(GameInitializer.CreateRolesForGame(players.Count).Cast<Role>());
            var characters = new Deck<Character>(GameInitializer.Characters.Cast<Character>());

            foreach (var player in players)
            {
                player.SetInfo(roles.Dequeue(), characters.Dequeue());
                FillPlayerHand(player);
            }
        }

        private void FillPlayerHand(Player player)
        {
            while (player.PlayerTablet.Health > player.Hand.Count)
            {
                if (deck.Count == 0)
                {
                    ResetDeck();
                }
                player.AddCardToHand(deck.Dequeue());
            }
        }

        private void ResetDeck()
        {
            deck = new Deck<BangGameCard>(discardedCards);
            discardedCards = new Stack<BangGameCard>();
        }
    }
}
