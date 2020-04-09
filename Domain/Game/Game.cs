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
    public class Game
    {
        public string Id { get; }
        public List<Player> Players { get; }
        public Gameplay Gameplay;

        private readonly object lockObj;
        
        public Game(Player player)
        {
            Id = Guid.NewGuid().ToString();
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
        
        public bool KickPlayer(Player player)
        {
            lock(lockObj)
            {
                if (Players.Count > 0)
                {
                    Players.Remove(player);
                    return true;
                }
                return false;
            }
        }

        public int GetPlayersAmount()
        {
            return Players.Count;
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

        public void Start()
        {
            Gameplay = new Gameplay();
            Gameplay.Initialize(Players);
        }
    }
}
