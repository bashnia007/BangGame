using Bang.Characters;
using Bang.GameEvents;
using Bang.Players;
using FluentAssertions;
using Server.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Server.Tests.Characters
{
    public class KitCarlsonSpecification
    {
        [Fact]
        public void Kit_Karlson_receives_choose_cards_response()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();
            nextPlayer.SetInfo(game.Gameplay, nextPlayer.Role, new KitCarlson());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            responseMessage.Should().BeOfType<ReplyActionMessage>();
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            replayActionMessage.Response.Should().BeOfType<ChooseCardsResponse>();
        }

        [Fact]
        public void Kit_Karlson_receives_3_cards_to_choose()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();
            nextPlayer.SetInfo(game.Gameplay, nextPlayer.Role, new KitCarlson());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            var response = (ChooseCardsResponse)replayActionMessage.Response;
            response.CardsToChoose.Count.Should().Be(3);
        }

        [Fact]
        public void After_Kit_Karlson_selected_2_cards_state_is_done()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();
            nextPlayer.SetInfo(game.Gameplay, nextPlayer.Role, new KitCarlson());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            var response = (ChooseCardsResponse)replayActionMessage.Response;

            var reply = responseMessage.ReplyWithChoosingCard(nextPlayer, response.CardsToChoose[0]);
            game.ProcessEvent(reply);
        }

        #region Private methods

        private Player CreatePlayer()
        {
            string id = Guid.NewGuid().ToString();
            Lobby.AddPlayer(id);
            Lobby.SetPlayerName(id, "Dr. Who");

            return Lobby.GetPlayer(id);
        }

        private Game CreateGame(Player player)
        {
            var game = new Game(player);
            return game;
        }

        private Game CreateAndStartGame(int playersCount = 4)
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < playersCount - 1; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }

        #endregion
    }
}
