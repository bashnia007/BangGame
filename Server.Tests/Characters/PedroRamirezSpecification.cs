using Bang.Characters;
using Bang.GameEvents;
using Bang.GameEvents.Enums;
using Bang.PlayingCards;
using FluentAssertions;
using Server.Messages;
using Xunit;

namespace Server.Tests.Characters
{
    public class PedroRamirezSpecification : CharactersSpecification
    {
        [Fact]
        public void Pedro_ramirez_receives_choose_draw_option_response()
        {
            var game = CreateAndStartGame();
            var pedroRamirez = game.Gameplay.GetNextPlayer();
            pedroRamirez.SetInfo(game.Gameplay, pedroRamirez.Role, new PedroRamirez());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            responseMessage.Should().BeOfType<ReplyActionMessage>();
            var replayActionMessage = (ReplyActionMessage)responseMessage;
            replayActionMessage.Response.Should().BeOfType<ChooseDrawOptionResponse>();
        }

        [Fact]
        public void After_Pedro_Ramirez_selected_draw_option_state_is_done()
        {
            var game = CreateAndStartGame();
            var pedroRamirez = game.Gameplay.GetNextPlayer();
            pedroRamirez.SetInfo(game.Gameplay, pedroRamirez.Role, new PedroRamirez());

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var reply = responseMessage.ReplyWithSelectedDrawOption(pedroRamirez, DrawOptions.FromDeck);
            var result = game.ProcessEvent(reply);
            result.Should().BeOfType<ActionDoneMessage>();
        }

        [Fact]
        public void Pedro_Ramirez_receives_card_from_deck_if_chose_this_option()
        {
            var game = CreateAndStartGame();
            var pedroRamirez = game.Gameplay.GetNextPlayer();
            pedroRamirez.SetInfo(game.Gameplay, pedroRamirez.Role, new PedroRamirez());

            var cardOnDeck = game.Gameplay.PeekTopCardFromDeck();

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var reply = responseMessage.ReplyWithSelectedDrawOption(pedroRamirez, DrawOptions.FromDeck);
            var result = game.ProcessEvent(reply);
            pedroRamirez.Hand.Should().Contain(cardOnDeck);
        }

        [Fact]
        public void Pedro_Ramirez_receives_card_from_discarded_if_chose_this_option()
        {
            var game = CreateAndStartGame();
            var pedroRamirez = game.Gameplay.GetNextPlayer();
            pedroRamirez.SetInfo(game.Gameplay, pedroRamirez.Role, new PedroRamirez());

            var cardOnDiscarded = new MissedCardType().SpadesQueen();
            game.Gameplay.DropCard(cardOnDiscarded);

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            var responseMessage = game.ProcessEvent(nextPlayerTurnMessage);
            var reply = responseMessage.ReplyWithSelectedDrawOption(pedroRamirez, DrawOptions.FromDiscard);
            var result = game.ProcessEvent(reply);
            pedroRamirez.Hand.Should().Contain(cardOnDiscarded);
        }
    }
}
