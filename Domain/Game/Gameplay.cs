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
        private Deck<BangGameCard> discardedCards;
        private List<Player> players;
        
        public void Initialize(List<Player> players)
        {
            this.players = players;
            
            deck = new Deck<BangGameCard>(GameInitializer.PlayingCards);
            deck.Shuffle();
            
            discardedCards = new Deck<BangGameCard>();
            
            ProvideCardsForPlayers();
            
            foreach (var player in this.players)
            {
                player.CardsDropped += DropCardsFromHand;
                player.CardsTaken += TakeCardsOnHand;
                player.PlayerTablet.CardDropped += DropCardsFromHand;
            }
        }

        public BangGameCard GetTopCardFromDiscarded()
        {
            if (discardedCards.IsEmpty()) return null;
            return discardedCards.Deal();
        }

        public BangGameCard DealCard()
        {
            if (deck.IsEmpty()) ResetDeck();

            return deck.Deal();
        }

        private void DropCardsFromHand(List<BangGameCard> cardsToDrop)
        {
            foreach (var card in cardsToDrop)
            {
                discardedCards.Put(card);
            }
        }

        private List<BangGameCard> TakeCardsOnHand(short amount, string playerId)
        {
            var result = new List<BangGameCard>();

            for (short i = 0; i < amount; i++)
            {
                result.Add(DealCard());
            }

            return result;
        }

        private void ProvideCardsForPlayers()
        {
            var roles = new Deck<Role>(GameInitializer.CreateRolesForGame(players.Count).Cast<Role>());
            var characters = new Deck<Character>(GameInitializer.Characters.Cast<Character>());

            foreach (var player in players)
            {
                player.SetInfo(roles.Deal(), characters.Deal());
                FillPlayerHand(player);
            }
        }

        private void FillPlayerHand(Player player)
        {
            while (player.PlayerTablet.Health > player.Hand.Count)
            {
                player.AddCardToHand(DealCard());
            }
        }

        private void ResetDeck()
        {
            deck = discardedCards.Shuffle();
            discardedCards = new Deck<BangGameCard>();
        }
    }
}
