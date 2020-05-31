using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class GeneralStoreSpecification
    {
        #region Tests
        [Fact]
        public void After_played_general_store_card_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GeneralStoreCard());

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(GeneralStoreCard());
        }

        [Fact]
        public void Player_discards_gatling_card_after_it_played()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GeneralStoreCard());

            // Assert
            actor.Hand.Should().NotContain(GeneralStoreCard());
        }

        [Fact]
        public void Player_who_played_general_store_card_is_the_first_who_can_choose()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            var response = actor.PlayCard(GeneralStoreCard());

            // Assert
            var chooseCardsResponse = (ChooseCardsResponse) response;
            chooseCardsResponse.PlayerTurn.Id.Should().Be(actor.Id);
        }

        [Fact]
        public void Store_has_the_same_cards_amount_as_players()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            var response = actor.PlayCard(GeneralStoreCard());

            // Assert
            var chooseCardsResponse = (ChooseCardsResponse)response;
            chooseCardsResponse.CardsToChoose.Count.Should().Be(gameplay.Players.Count);
        }

        [Fact]
        public void When_player_choose_card_from_the_store_it_comes_to_hand()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            var response = actor.PlayCard(GeneralStoreCard());

            // Assert
            var chooseCardsResponse = (ChooseCardsResponse)response;
            var card = chooseCardsResponse.CardsToChoose[0];

            actor.ChooseCard(card);

            actor.Hand.Should().Contain(card);
        }

        #endregion

        #region Private methods

        private Game.Gameplay InitGame() => InitGame(BangGameDeck());

        private Game.Gameplay InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }

            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);
            return gameplay;
        }

        private Player ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(GeneralStoreCard());

            return actor;
        }

        private BangGameCard GeneralStoreCard() => new GeneralStoreCardType().SpadesQueen();

        #endregion
    }

}
