using Domain.Players;
using Domain.PlayingCards;
using System;
using System.Collections.Generic;

namespace Domain.Game
{
    public class Game
    {
        public string Id { get; }
        public List<Player> Players { get; }
        public Queue<PlayingCard> Deck { get; set; }

        public Game(List<Player> players)
        {
            Id = Guid.NewGuid().ToString();
            Players = players;
        }

        public void Initialize()
        {
            var gameSet = GameInitializer.CreateGameSet(Players.Count);
            Deck = gameSet.Deck;

            foreach (var player in Players)
            {
                player.SetInfo(gameSet.Roles.Dequeue(), gameSet.Characters.Dequeue());
                FillPlayerHand(player);
            }
        }

        private void FillPlayerHand(Player player)
        {
            while(player.PlayerTablet.Health > player.PlayerHand.Count)
            {
                player.PlayerHand.Add(Deck.Dequeue());
            }
        }
    }
}
