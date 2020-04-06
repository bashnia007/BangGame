using Domain.Game;
using Domain.Players;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    static class Lobby
    {
        private static BlockingCollection<Game> games = new BlockingCollection<Game>();
        private static BlockingCollection<Player> players = new BlockingCollection<Player>();

        public static List<Game> GetGames()
        {
            return games.ToList();
        }

        public static void AddGame(Game game)
        {
            games.TryAdd(game);
        }

        public static Game GetGame(string gameId)
        {
            return games.FirstOrDefault(g => g.Id == gameId);
        }

        public static void CloseGame(string gameId)
        {
            var game = GetGame(gameId);
            games.TryTake(out game);
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
