using Domain.Characters;
using Domain.Players;
using Domain.PlayingCards;
using Domain.Roles;

using System;
using System.Collections.Generic;
using System.Linq;


namespace Domain.Game
{
    public class Game
    {
        public string Id { get; }
        public List<Player> Players { get; }
        public Deck<PlayingCard> Deck { get; set; }
        public Stack<PlayingCard> DiscardedCards { get; set; }

        public Game(List<Player> players)
        {
            Id = Guid.NewGuid().ToString();
            Players = players;
            DiscardedCards = new Stack<PlayingCard>();
            Deck = new Deck<PlayingCard>();
        }

        public void Initialize()
        {
            Deck = new Deck<PlayingCard>(GameInitializer.PlayingCards.Cast<PlayingCard>());
            var roles = new Deck<Role>(GameInitializer.CreateRolesForGame(Players.Count).Cast<Role>());
            var characters = new Deck<Character>(GameInitializer.Characters.Cast<Character>());

            foreach (var player in Players)
            {
                player.SetInfo(roles.Dequeue(), characters.Dequeue());
                FillPlayerHand(player);
            }
        }

        private void FillPlayerHand(Player player)
        {
            while(player.PlayerTablet.Health > player.PlayerHand.Count)
            {
                if (Deck.Count == 0)
                {
                    ResetDeck();
                }
                player.PlayerHand.Add(Deck.Dequeue());
            }
        }

        private void ResetDeck()
        {
            Deck = new Deck<PlayingCard>(DiscardedCards);
            DiscardedCards = new Stack<PlayingCard>();
        }
    }
}
