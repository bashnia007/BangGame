﻿using Domain.Players;
using NLog;
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
        public bool IsStarted { get; private set; }

        private readonly object lockObj;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Game(Player player)
        {
            Id = Guid.NewGuid().ToString();
            Players = new List<Player>();
            lockObj = new object();
            Players.Add(player);
            IsStarted = false;
        }
        
        public bool JoinPlayer(Player player)
        {
            lock(lockObj)
            {
                if (Players.Count < 7)
                {
                    Logger.Debug($"Player {player.Id} was joined");
                    Players.Add(player);
                    return true;
                }
                Logger.Debug($"Can't join player. Game already has maximum amount of players");
                return false;
            }
        }
        
        public bool KickPlayer(Player player)
        {
            lock(lockObj)
            {
                if (Players.Count > 0)
                {
                    Logger.Debug($"Player {player.Id} was kicked");
                    Players.Remove(player);
                    return true;
                }
                Logger.Warn($"Player {player.Id} wasn't kicked. Amount of players = {Players.Count}");
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
            IsStarted = true;
        }
    }
}
