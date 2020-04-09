﻿using Domain.Game;
using Domain.Players;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public static class Lobby
    {
        private static List<Game> games = new List<Game>();
        private static BlockingCollection<Player> players = new BlockingCollection<Player>();
        private static object lockObject = new object();

        public static List<Game> GetGames()
        {
            return games.ToList();
        }

        public static void AddGame(Game game)
        {
            lock(lockObject)
            {
                games.Add(game);
            }
        }

        public static Game GetGame(string gameId)
        {
            return games.FirstOrDefault(g => g.Id == gameId);
        }

        public static void CloseGame(string gameId)
        {
            lock(lockObject)
            {
                var game = GetGame(gameId);
                games.Remove(game);
            }
        }

        public static void AddPlayer(string playerId)
        {
            players.TryAdd(new PlayerOnline(playerId));
        }

        public static void SetPlayerName(string playerId, string name)
        {
            GetPlayer(playerId).Name = name;
        }

        public static Player GetPlayer(string playerId)
        {
            return players.First(p => p.Id == playerId);
        }
    }
}
