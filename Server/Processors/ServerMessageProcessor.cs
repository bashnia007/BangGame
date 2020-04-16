using Domain.Game;
using Domain.Messages;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Processors
{
    public class ServerMessageProcessor : IMessageProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<Message> ProcessConnectedMessage(ConnectionMessage message)
        {
            Logger.Debug("Received Connection Message");

            var result = new List<Message>();
            
            Lobby.SetPlayerName(message.PlayerId, message.Name);

            return result;
        }

        public List<Message> ProcessCreateGameMessage(CreateGameMessage message)
        {
            Logger.Debug("Received Create Game Message");

            var result = new List<Message>();

            var player = Lobby.GetPlayer(message.PlayerId);
            var game = new Game(player);
            Lobby.AddGame(game);

            message.GameId = game.Id;
            result.Add(message);

            return result;
        }

        public List<Message> ProcessGetGamesMessage(GetGamesMessage message)
        {
            Logger.Debug("Received Get Games Message");

            var result = new List<Message>();
            message.Games = Lobby.GetGames();
            result.Add(message);

            return result;
        }

        public List<Message> ProcessJoinGameMessage(JoinGameMessage message)
        {
            Logger.Debug("Received Join Game Message");

            var result = new List<Message>();

            var player = Lobby.GetPlayer(message.PlayerId);
            var game = Lobby.GetGame(message.GameId);

            message.IsJoined = game.JoinPlayer(player);
            Logger.Debug("Player joined: " + message.IsJoined);

            result.Add(message);

            return result;
        }

        public List<Message> ProcessLeaveGameMessage(LeaveGameMessage message)
        {
            Logger.Debug("Received Leave Game Message");
            var result = new List<Message>();
            
            var player = Lobby.GetPlayer(message.PlayerId);
            var game = Lobby.GetGame(message.GameId);

            message.IsSuccess = game.KickPlayer(player);
            result.Add(message);

            if (game.GetPlayersAmount() == 0)
            {
                Logger.Debug($"Closing game with id={game.Id} due to empty list of players");
                Lobby.CloseGame(game.Id);
            }

            return result;
        }

        public List<Message> ProcessReadyToPlayMessage(ReadyToPlayMessage message)
        {
            Logger.Debug("Received Ready to Play Message");

            var result = new List<Message>();

            var game = Lobby.GetGame(message.GameId);
            game.SetPlayerReadyStatus(message.PlayerId, message.IsReady);

            if (game.AllPlayersAreReady())
            {
                Logger.Debug("All players are ready. Let's start!");

                game.Start();

                foreach (var player in game.Players)
                {
                    var startGameMessage = new StartGameMessage(player.Role, player.PlayerTablet.Character, 
                        player.PlayerHand, game.Id, player.Id);

                    result.Add(startGameMessage);
                    Logger.Debug($"Player with id={player.Id} received: {player.Role.Description} role " +
                        $"and {player.PlayerTablet.Character} character");
                }
            }

            return result;
        }

        public List<Message> ProcessStartGameMessage(StartGameMessage message)
        {
            throw new NotImplementedException();
        }

        public List<Message> ProcessTakeCardsMessage(TakeCardsMessage message)
        {
            var result = new List<Message>();

            var game = Lobby.GetGame(message.GameId);
            var player = game.Players.First(p => p.Id == message.PlayerId);
            message = new TakeCardsMessage(player.TakeCards(message.CardsToTakeAmount));
            result.Add(message);

            return result;
        }

        public List<Message> ProcessDropCardsMessage(DropCardsMessage message)
        {
            var result = new List<Message>();

            var game = Lobby.GetGame(message.GameId);
            var player = game.Players.First(p => p.Id == message.PlayerId);
            player.DropCards(message.CardsToDrop);

            return result;
        }

        public List<Message> ProcessLongTermFeatureCardMessage(LongTermFeatureCardMessage message)
        {
            var result = new List<Message>();

            var game = Lobby.GetGame(message.GameId);
            var player = game.Players.First(p => p.Id == message.PlayerId);
            if (player.PlayerTablet.CanPutCard(message.CardForTablet))
            {
                player.PlayerTablet.PutCard(message.CardForTablet);
                player.PlayerHand.Remove(message.CardForTablet);
                message.IsSuccess = true;
            }
            else
            {
                message.IsSuccess = false;
            }

            result.Add(message);

            return result;
        }
    }
}
