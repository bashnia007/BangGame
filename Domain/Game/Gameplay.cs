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
        private Deck<PlayingCard> deck;
        private Stack<PlayingCard> discardedCards;
        private List<Player> players;
        
        public void Initialize(List<Player> players)
        {
            discardedCards = new Stack<PlayingCard>();
            this.players = players;
            deck = new Deck<PlayingCard>(GameInitializer.PlayingCards.Cast<PlayingCard>());

            ProvideCardsForPlayers();
        }

        public void DropCardsFromHand(List<PlayingCard> cardsToDrop, string playerId)
        {
            var player = players.First(p => p.Id == playerId);
            foreach (var card in cardsToDrop)
            {
                discardedCards.Push(card);
                player.PlayerHand.Remove(card);
            }
        }

        public List<PlayingCard> TakeCardsOnHand(short amount, string playerId)
        {
            List<PlayingCard> result = new List<PlayingCard>();
            var player = players.First(p => p.Id == playerId);

            for (short i = 0; i < amount; i++)
            {
                if (deck.Count == 0)
                {
                    ResetDeck();
                }
                var card = deck.Dequeue();
                result.Add(card);
                player.PlayerHand.Add(card);
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
            while (player.PlayerTablet.Health > player.PlayerHand.Count)
            {
                if (deck.Count == 0)
                {
                    ResetDeck();
                }
                player.PlayerHand.Add(deck.Dequeue());
            }
        }

        private void ResetDeck()
        {
            deck = new Deck<PlayingCard>(discardedCards);
            discardedCards = new Stack<PlayingCard>();
        }
    }
}
