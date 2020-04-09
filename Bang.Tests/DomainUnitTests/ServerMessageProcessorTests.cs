﻿using Domain.Game;
using Domain.Messages;
using Domain.Players;
using Server;
using Server.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class ServerMessageProcessorTests
    {
        [Fact]
        public void Connected_player_sets_his_name()
        {
            var player = CreatePlayer();
            var connectionMessage = new ConnectionMessage();
            connectionMessage.PlayerId = player.Id;
            connectionMessage.Name = "Dr. Who";

            var serverProcessor = new ServerMessageProcessor();
            serverProcessor.ProcessConnectedMessage(connectionMessage);

            Assert.Equal(connectionMessage.Name, Lobby.GetPlayer(player.Id).Name);
        }

        [Fact]
        public void Create_game_message_creates_a_new_game()
        {
            var player = CreatePlayer();
            var createGameMessage = new CreateGameMessage();
            createGameMessage.PlayerId = player.Id;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessCreateGameMessage(createGameMessage);
            var responseCreateGameMessage = response.First() as CreateGameMessage;

            Assert.NotNull(responseCreateGameMessage);
            Assert.NotNull(Lobby.GetGame(responseCreateGameMessage.GameId));
        }

        [Fact]
        public void Get_games_message_returns_list_of_games()
        {
            CleanLobby();
            var player = CreatePlayer();
            var getGamesMessage = new GetGamesMessage();
            getGamesMessage.PlayerId = player.Id;

            var games = CreateGames(3, player);

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessGetGamesMessage(getGamesMessage);
            var responseGetGamesMessage = response.First() as GetGamesMessage;

            Assert.Equal(games.Select(g => g.Id), responseGetGamesMessage.Games.Select(g => g.Id));
        }

        [Fact]
        public void Player_can_join_into_game()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            var joinGameMessage = new JoinGameMessage();
            joinGameMessage.PlayerId = player.Id;
            joinGameMessage.GameId = game.Id;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessJoinGameMessage(joinGameMessage);
            var responseJoinGameMessage = response.First() as JoinGameMessage;

            Assert.Equal(2, Lobby.GetGame(game.Id).GetPlayersAmount());
            Assert.True(responseJoinGameMessage.IsJoined);
        }

        [Fact]
        public void Cannot_join_into_full_game()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);
            for (int i = 0; i < 6; i++)
            {
                game.JoinPlayer(player);
            }

            var joinGameMessage = new JoinGameMessage();
            joinGameMessage.PlayerId = player.Id;
            joinGameMessage.GameId = game.Id;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessJoinGameMessage(joinGameMessage);
            var responseJoinGameMessage = response.First() as JoinGameMessage;

            Assert.False(responseJoinGameMessage.IsJoined);
        }

        [Fact]
        public void Player_can_leave_game()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            var player2 = CreatePlayer();
            game.JoinPlayer(player2);

            var leaveGameMessage = new LeaveGameMessage();
            leaveGameMessage.GameId = game.Id;
            leaveGameMessage.PlayerId = player2.Id;

            var serverProcessor = new ServerMessageProcessor();
            serverProcessor.ProcessLeaveGameMessage(leaveGameMessage);

            Assert.DoesNotContain(player2, game.Players);
        }

        [Fact]
        public void When_the_last_player_leaves_the_game_closes()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);
            
            var leaveGameMessage = new LeaveGameMessage();
            leaveGameMessage.GameId = game.Id;
            leaveGameMessage.PlayerId = player.Id;

            var serverProcessor = new ServerMessageProcessor();
            serverProcessor.ProcessLeaveGameMessage(leaveGameMessage);

            Assert.DoesNotContain(game, Lobby.GetGames());
        }

        [Fact]
        public void Ready_to_play_message_changes_player_status()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            var readyToPlayMessage = new ReadyToPlayMessage();
            readyToPlayMessage.GameId = game.Id;
            readyToPlayMessage.PlayerId = player.Id;
            readyToPlayMessage.IsReady = true;

            var serverProcessor = new ServerMessageProcessor();
            serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);
            Assert.True(game.Players.First(p => p.Id == player.Id).IsReadyToPlay);

            readyToPlayMessage.IsReady = false;
            serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);
            Assert.False(game.Players.First(p => p.Id == player.Id).IsReadyToPlay);
        }

        [Fact]
        public void When_all_players_are_ready_players_receive_cards()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            foreach(var gamePlayer in game.Players)
            {
                gamePlayer.IsReadyToPlay = true;
            }

            var newPlayer = CreatePlayer();
            game.JoinPlayer(newPlayer);

            var readyToPlayMessage = new ReadyToPlayMessage();
            readyToPlayMessage.GameId = game.Id;
            readyToPlayMessage.PlayerId = newPlayer.Id;
            readyToPlayMessage.IsReady = true;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);
            
            foreach (StartGameMessage message in response)
            {
                Assert.NotNull(message.Role);
                Assert.NotNull(message.Character);
                Assert.NotNull(message.Hand);
            }
        }

        [Fact]
        public void When_all_players_are_ready_each_player_receives_message()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            foreach (var gamePlayer in game.Players)
            {
                gamePlayer.IsReadyToPlay = true;
            }

            var newPlayer = CreatePlayer();
            game.JoinPlayer(newPlayer);

            var readyToPlayMessage = new ReadyToPlayMessage();
            readyToPlayMessage.GameId = game.Id;
            readyToPlayMessage.PlayerId = newPlayer.Id;
            readyToPlayMessage.IsReady = true;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);

            Assert.Equal(game.Players.Count, response.Count);
        }

        [Fact]
        public void When_all_players_are_ready_close_game()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            foreach (var gamePlayer in game.Players)
            {
                gamePlayer.IsReadyToPlay = true;
            }

            var newPlayer = CreatePlayer();
            game.JoinPlayer(newPlayer);

            var readyToPlayMessage = new ReadyToPlayMessage();
            readyToPlayMessage.GameId = game.Id;
            readyToPlayMessage.PlayerId = newPlayer.Id;
            readyToPlayMessage.IsReady = true;

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);

            Assert.DoesNotContain(game, Lobby.GetGames());
        }

        private Player CreatePlayer()
        {
            string id = Guid.NewGuid().ToString();
            Lobby.AddPlayer(id);
            Lobby.SetPlayerName(id, "Dr. Who");

            return Lobby.GetPlayer(id);
        }

        private List<Game> CreateGames(int amount, Player player)
        {
            var result = new List<Game>();
            for (int i = 0; i < amount; i++)
            {
                result.Add(CreateGame(player));
            }

            return result;
        }

        private Game CreateGame(Player player)
        {
            var game = new Game(player);
            Lobby.AddGame(game);
            return game;
        }

        private void CleanLobby()
        {
            foreach(var game in Lobby.GetGames())
            {
                Lobby.CloseGame(game.Id);
            }
        }
    }
}
