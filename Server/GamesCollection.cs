using Domain.Game;
using Domain.Players;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    static class GamesCollection
    {
        private static BlockingCollection<Game> games = new BlockingCollection<Game>();

        public static List<Game> GetGames()
        {
            return games.ToList();
        }

        public static void AddGame(Game game)
        {
            games.TryAdd(game);
        }

        public static void JoinPlayerToGame(Player player, string gameId)
        {
            var game = GetGame(gameId);

            if (game == null) throw new ArgumentNullException(gameId);

            game.JoinPlayer(player);
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
    }
}
