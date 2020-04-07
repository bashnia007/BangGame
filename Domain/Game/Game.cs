using Domain.Exceptions;
using Domain.Players;
using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    [Serializable]
    public class Game
    {
        public string Id { get; }
        public List<Player> Players { get; }
        public Deck<PlayingCard> Deck { get; set; }
        public Stack<PlayingCard> DiscardedCards { get; set; }

        private object lockObj;
        
        public Game(Player player)
        {
            Id = Guid.NewGuid().ToString();
            DiscardedCards = new Stack<PlayingCard>();
            Deck = new Deck<PlayingCard>();
            Players = new List<Player>();
            lockObj = new object();
            Players.Add(player);
        }
        
        public bool JoinPlayer(Player player)
        {
            lock(lockObj)
            {
                if (Players.Count < 7)
                {
                    Players.Add(player);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// removes player from list of players in the game
        /// </summary>
        /// <param name="player">player to be removed</param>
        /// <returns>is players count greater than zero</returns>
        public bool KickPlayer(Player player)
        {
            lock(lockObj)
            {
                if (Players.Count > 0)
                {
                    Players.Remove(player);
                }
                return Players.Count > 0;
            }
        }

        public void SetPlayerReadyStatus(string playerId, bool readyStatus)
        {
            var player = Players.First(p => p.Id == playerId);
            player.IsReadyToPlay = readyStatus;
        }

        public bool AllPlayersAreReady()
        {
            return Players.All(p => p.IsReadyToPlay) && Players.Count > 3;
        }

        public void Initialize()
        {
            Deck = new Deck<PlayingCard>(GameInitializer.PlayingCards.Cast<PlayingCard>());
            var roles = new Deck<Role.Role>(GameInitializer.CreateRolesForGame(Players.Count).Cast<Role.Role>());
            var characters = new Deck<Character.Character>(GameInitializer.Characters.Cast<Character.Character>());

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
