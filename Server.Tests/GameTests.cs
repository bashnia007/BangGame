using System;
using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;

namespace Server.Tests
{
    public class GameTests
    {
        #region Private methods

        private Player CreatePlayer()
        {
            var id = new Guid().ToString();
            return new PlayerOnline(id);
        }

        private List<Player> CreatePlayers(int amount)
        {
            var result = new List<Player>();

            for (int i = 0; i < amount; i++)
            {
                result.Add(CreatePlayer());
            }

            return result;
        }

        private List<CardType> CreatePlayingCardsList()
        {
            return new List<CardType>
            {
                new BangCardType(),
                new MissedCardType(),
                new DynamiteCardType(),
            };
        }

        private global::Server.Game CreateAndStartGame()
        {
            var player = CreatePlayer();
            var game = new global::Server.Game(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }

        #endregion
    }
}
