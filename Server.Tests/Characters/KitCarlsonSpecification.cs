using Bang.Characters;
using Bang.GameEvents;
using FluentAssertions;
using Server.Messages;
using Xunit;

namespace Server.Tests.Characters
{
    public class KitCarlsonSpecification : CharactersSpecification
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
    }
}
