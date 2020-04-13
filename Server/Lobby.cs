using Domain.Game;
using Domain.Players;
using NLog;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns list of not started games
        /// </summary>
        /// <returns></returns>
        public static List<Game> GetGames()
        {
            return games.Where(g => !g.IsStarted).ToList();
        }

        public static void AddGame(Game game)
        {
            lock(lockObject)
            {
                Logger.Debug("Adding new game with id=" + game.Id);
                games.Add(game);
                Logger.Debug("Game was added");
            }
        }

        /// <summary>
        /// Returns specific game, even started, by id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static Game GetGame(string gameId)
        {
            return games.FirstOrDefault(g => g.Id == gameId);
        }

        /// <summary>
        /// Removes game from lobby
        /// </summary>
        /// <param name="gameId"></param>
        public static void CloseGame(string gameId)
        {
            lock(lockObject)
            {
                Logger.Debug("Closing game with id=" + gameId);
                var game = GetGame(gameId);
                games.Remove(game);
                Logger.Debug("Game was removed");
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
