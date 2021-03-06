﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Players;
using Xunit;

namespace Server.Tests
{
    [Collection("Lobby affecting collection")]
    public class LobbyTests
    {
        [Fact]
        public void Game_can_be_added_into_lobby()
        {
            var player = CreatePlayer();
            var game1 = new Game(player);
            var game2 = new Game(player);

            Lobby.AddGame(game1);
            Lobby.AddGame(game2);

            Assert.Equal(game1, Lobby.GetGame(game1.Id));
            Assert.Equal(game2, Lobby.GetGame(game2.Id));
        }

        [Fact]
        public void Get_games_returns_the_actual_list_of_games()
        {
            var player = CreatePlayer();
            const int gamesAmount = 3;
            List<string> gameIds = new List<string>();

            CleanLobby();

            for (int i = 0; i < gamesAmount; i++)
            {
                var game = new Game(player);
                gameIds.Add(game.Id);
                Lobby.AddGame(game);
            }

            Assert.Equal(gameIds, Lobby.GetGames().Select(g => g.Id));
        }
        
        [Fact]
        public void Player_can_be_added_into_lobby()
        {
            var player = CreatePlayer();
                        
            Lobby.AddPlayer(player.Id);

            Assert.Equal(Lobby.GetPlayer(player.Id).Id, player.Id);
        }

        [Fact]
        public void Player_name_can_be_changed()
        {
            var player = CreatePlayer();
            string playerName = "Dr. Who";

            Lobby.AddPlayer(player.Id);
            Lobby.SetPlayerName(player.Id, playerName);

            Assert.Equal(playerName, Lobby.GetPlayer(player.Id).Name);
        }

        [Fact]
        public void Game_can_be_closed()
        {
            var player = CreatePlayer();
            var game = new Game(player);
            Lobby.AddGame(game);

            Assert.Equal(game, Lobby.GetGame(game.Id));

            Lobby.CloseGame(game.Id);

            Assert.Null(Lobby.GetGame(game.Id));
        }

        private PlayerOnline CreatePlayer()
        {
            var playerId = Guid.NewGuid().ToString();
            return new PlayerOnline(playerId);
        }

        private void CleanLobby()
        {
            foreach (var game in Lobby.GetGames())
            {
                Lobby.CloseGame(game.Id);
            }
        }
    }
}
