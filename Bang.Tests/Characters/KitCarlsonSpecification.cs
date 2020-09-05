using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests.Characters
{
    public class KitCarlsonSpecification
    {
        #region Tests

        [Fact]
        public void Kit_Carlson_starts_game_with_4_life_points()
        {
            var (gamePlay, kitCarlson) = InitGame();
            kitCarlson.AsDeputy(gamePlay);

            kitCarlson.LifePoints.Should().Be(4);
        }

        [Fact]
        public void Kit_Carlson_receives_three_cards_to_choose()
        {
            var (gamePlay, kitCarlson) = InitGame();

            gamePlay.PutCardOnDeck(BangCard());
            gamePlay.PutCardOnDeck(MissedCard());
            gamePlay.PutCardOnDeck(MustangCard());
            var response = gamePlay.StartPlayerTurn();

            // Assertion
            response.Should().BeOfType<ChooseCardsResponse>();
            response.As<ChooseCardsResponse>().CardsToChoose.Count.Should().Be(3);
        }

        [Fact]
        public void Declined_card_by_Kit_Carlson_is_going_back_to_deck()
        {
            var (gamePlay, kitCarlson) = InitGame();
            
            gamePlay.PutCardOnDeck(BangCard());
            gamePlay.PutCardOnDeck(MissedCard());
            gamePlay.PutCardOnDeck(MustangCard());

            var response = gamePlay.StartPlayerTurn();
            kitCarlson.ChooseCardToReturn(gamePlay, MissedCard());
            gamePlay.PeekTopCardFromDeck().Should().Be(MissedCard());
        }

        [Fact]
        public void Kit_Carlson_leaves_two_cards_after_selection()
        {
            var (gamePlay, kitCarlson) = InitGame();
            
            gamePlay.PutCardOnDeck(BangCard());
            gamePlay.PutCardOnDeck(MissedCard());
            gamePlay.PutCardOnDeck(MustangCard());

            var response = gamePlay.StartPlayerTurn();
            kitCarlson.ChooseCardToReturn(gamePlay, MissedCard());
            kitCarlson.Hand.Count.Should().Be(2);
        }

        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();
        private BangGameCard MustangCard() => new MustangCardType().SpadesQueen();

        private (Gameplay, Player) InitGame(Deck<BangGameCard> deck = null)
        {
            var kitKarlson = new KitCarlson();
            var gameplayBuilder = new GameplayBuilder().WithCharacter(kitKarlson);

            if (deck != null)
                gameplayBuilder.WithDeck(deck);
                
            var gameplay = gameplayBuilder.Build();

            var player = gameplay.FindPlayer(kitKarlson);
            gameplay.SetTurnToPlayer(player);
            
            return (gameplay, player);
        }

        #endregion
    }
}
