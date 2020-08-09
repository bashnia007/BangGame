using Bang.Characters;
using Bang.GameEvents;
using Bang.Players;
using FluentAssertions;
using Server.Messages;
using System;
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
            response.CardsToChoose.Should().HaveCount(3);
        }

        [Fact]
        public void After_Kit_Karlson_selected_card_to_return_state_is_done()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();
            nextPlayer.SetInfo(game.Gameplay, nextPlayer.Role, new KitCarlson());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            var response = (ChooseCardsResponse)replayActionMessage.Response;

            var reply = responseMessage.ReplyWithChoosingCard(nextPlayer, response.CardsToChoose[0]);
            var result = game.ProcessEvent(reply);
            result.Should().BeOfType<ActionDoneMessage>();
        }

        [Fact]
        public void After_Kit_Karlson_selected_card_to_return_he_has_only_two_remaining_cards()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();
            nextPlayer.SetInfo(game.Gameplay, nextPlayer.Role, new KitCarlson());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            var response = (ChooseCardsResponse)replayActionMessage.Response;

            var reply = responseMessage.ReplyWithChoosingCard(nextPlayer, response.CardsToChoose[0]);
            var result = game.ProcessEvent(reply);
            nextPlayer.Hand.Should().Contain(response.CardsToChoose[1]);
            nextPlayer.Hand.Should().Contain(response.CardsToChoose[2]);
            nextPlayer.Hand.Should().NotContain(response.CardsToChoose[0]);
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
