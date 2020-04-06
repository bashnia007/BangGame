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

        #region Constructors

        private Game()
        {
            Id = Guid.NewGuid().ToString();
            DiscardedCards = new Stack<PlayingCard>();
            Deck = new Deck<PlayingCard>();
            Players = new List<Player>();
        }

        public Game(Player player) : this()
        {
            Players.Add(player);
        }

        public Game(List<Player> players) : this()
        {
            Players.AddRange(players);
        }

        #endregion

        public void JoinPlayer(Player player)
        {
            Players.Add(player);
        }

        public void SetPlayerReadyStatus(string playerId, bool readyStatus)
        {
            var player = Players.First(p => p.Id == playerId);
            player.IsReadyToPlay = readyStatus;
        }

        public bool AllPlayersAreReady()
        {
            return Players.All(p => p.IsReadyToPlay);
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
