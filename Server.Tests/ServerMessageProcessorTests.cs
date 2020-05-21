﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Messages;
using FluentAssertions;
using Server.Messages;
using Server.Processors;
using Xunit;

namespace Server.Tests
{
    [Collection("Lobby affecting collection")]
    public class ServerMessageProcessorTests
    {
        #region Tests

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

            var readyToPlayMessage = new ReadyToPlayMessage {GameId = game.Id, PlayerId = player.Id, IsReady = true};

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

            var readyToPlayMessage = new ReadyToPlayMessage {GameId = game.Id, PlayerId = newPlayer.Id, IsReady = true};

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

            var readyToPlayMessage = new ReadyToPlayMessage {GameId = game.Id, PlayerId = newPlayer.Id, IsReady = true};

            var serverProcessor = new ServerMessageProcessor();
            var response = serverProcessor.ProcessReadyToPlayMessage(readyToPlayMessage);

            Assert.Equal(game.Players.Count, response.Count);
        }

        [Fact]
        public void When_all_players_are_ready_and_game_started_it_is_not_visible_in_lobby()
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

        [Fact]
        public void Drop_cards_message_drops_cards_from_hand()
        {
            var game = CreateAndStartGame();
            var player = game.Players.First();
            var cardsToDrop = player.Hand.Take(1).ToList();
            var message = new DropCardsMessage(cardsToDrop, player);

            int cardsBeforeDrop = player.Hand.Count;

            game.ProcessEvent(message);

            Assert.Equal(cardsBeforeDrop - 1, player.Hand.Count);
        }

        [Fact]
        public void Drop_cards_message_adds_card_into_discarded()
        {
            var game = CreateAndStartGame();
            var player = game.Players.First();
            var cardsToDrop = player.Hand.Take(1).ToList();
            var message = new DropCardsMessage(cardsToDrop, player);

            // Act
            game.ProcessEvent(message);

            // Assert
            Assert.Equal(cardsToDrop.First(), game.Gameplay.GetTopCardFromDiscarded());
        }
        
        [Fact]
        public void Take_cards_message_adds_cards_to_hand()
        {
            const int cardsToTake = 3;

            var game = CreateAndStartGame();
            var player = game.Players.First();
            var message = new TakeCardsEvent(cardsToTake, player);

            var cardsAmountBeforeMessage = player.Hand.Count;

            game.ProcessEvent(message);

            Assert.Equal(cardsAmountBeforeMessage + cardsToTake, player.Hand.Count);
        }


        [Fact]
        public void Change_weapon_message_removes_card_from_hand()
        {
            var game = CreateAndStartGame();
            var player = game.Players.First();
            var card = new VolcanicCardType().ClubsSeven();
            player.AddCardToHand(card);

            player.PlayCard(card);

            Assert.DoesNotContain(card, player.Hand);
        }

        [Theory]
        [MemberData(nameof(ReplenishCardsToCardsAmountMapping))]
        public void Replenish_hand_card_message_returns_properly_cards_amount_in_message(BangGameCard card, int cardsShouldBeAdded)
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            player.AddCardToHand(card);

            var hand = player.Hand.Count;
            
            player.PlayCard(card);

            player.Hand.Count.Should().Be(hand - 1 + cardsShouldBeAdded);
        }

        [Theory]
        [MemberData(nameof(ReplenishCards))]
        public void Replenish_hand_card_message_removes_used_card_from_hand(BangGameCard card)
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            player.AddCardToHand(card);
            
            player.PlayCard(card);

            Assert.DoesNotContain(card, player.Hand);
        }
        
        #endregion

        #region Private methods

        private Player CreatePlayer()
        {
            string id = Guid.NewGuid().ToString();
            Lobby.AddPlayer(id);
            Lobby.SetPlayerName(id, "Dr. Who");

            return Lobby.GetPlayer(id);
        }

        private List<global::Server.Game> CreateGames(int amount, Player player)
        {
            var result = new List<global::Server.Game>();
            for (int i = 0; i < amount; i++)
            {
                result.Add(CreateGame(player));
            }

            return result;
        }

        private global::Server.Game CreateGame(Player player)
        {
            var game = new global::Server.Game(player);
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

        private global::Server.Game CreateAndStartGame()
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }
        
        #endregion

        #region Fields

        public static IEnumerable<object[]> ReplenishCardsToCardsAmountMapping
        {
            get
            {
                return new[]
                {
                    new object[] { new StagecoachCardType().ClubsSeven(), 2},
                    new object[] { new WellsFargoCardType().ClubsSeven(), 3,}
                };
            }
        }

        public static IEnumerable<object[]> ReplenishCards
        {
            get
            {
                return new[]
                {
                    new object[] { new StagecoachCardType().ClubsSeven(), },
                    new object[] { new WellsFargoCardType().ClubsSeven(), },
                };
            }
        }

        #endregion
    }
}