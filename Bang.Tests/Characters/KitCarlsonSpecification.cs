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
            var gamePlay = InitGame();
            if (gamePlay.PlayerTurn.Role is Sheriff)
                gamePlay.SetNextPlayer();
            var kitCarlson = SetCharacter(gamePlay, new KitCarlson());

            kitCarlson.LifePoints.Should().Be(4);
        }

        [Fact]
        public void Kit_Carlson_receives_three_cards_to_choose()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(BangCard());
            deck.Put(MissedCard());
            deck.Put(MustangCard());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new KitCarlson());

            var response = gamePlay.GivePhaseOneCards();

            // Assertion
            response.Should().BeOfType<ChooseCardsResponse>();
            response.As<ChooseCardsResponse>().CardsToChoose.Count.Should().Be(3);
        }

        [Fact]
        public void Declined_card_by_Kit_Carlson_is_going_back_to_deck()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(BangCard());
            deck.Put(MissedCard());
            deck.Put(MustangCard());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new KitCarlson());

            var response = gamePlay.GivePhaseOneCards();
            actor.ChooseCard(MissedCard());
            gamePlay.GetTopCardFromDeck().Should().Be(MissedCard());
        }

        [Fact]
        public void Kit_Carlson_leaves_two_cards_after_selection()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(BangCard());
            deck.Put(MissedCard());
            deck.Put(MustangCard());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new KitCarlson());

            var response = gamePlay.GivePhaseOneCards();
            actor.ChooseCard(MissedCard());
            actor.Hand.Count.Should().Be(2);
        }

        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();
        private BangGameCard MustangCard() => new MustangCardType().SpadesQueen();

        private Gameplay InitGame() => InitGame(BangGameDeck());

        private Gameplay InitGame(Deck<BangGameCard> deck)
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

        private Player SetCharacter(Game.Gameplay gameplay, Character character)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, character);

            return actor;
        }

        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(BangCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new Jourdonnais());

            return (actor, victim);
        }

        #endregion
    }
}
