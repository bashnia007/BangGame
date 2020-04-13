using Domain.Game;
using Domain.Players;
using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bang.Tests.DomainUnitTests
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

        private List<PlayingCard> CreatePlayingCardsList()
        {
            return new List<PlayingCard>
            {
                new BangCard(),
                new MissedCard(),
                new DynamiteCard(),
            };
        }

        private Game CreateAndStartGame()
        {
            var player = CreatePlayer();
            var game = new Game(player);

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
