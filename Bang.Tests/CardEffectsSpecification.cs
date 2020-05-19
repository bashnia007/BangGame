using System;
using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;
using Bang.Weapons;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class CardEffectsSpecification
    {
        [Fact]
        public void Played_card_goes_to_discard_pile()
        {
            var gamePlay = InitGame();
            var player = gamePlay.PlayerTurn;

            var stagecoach = new StagecoachCardType().SpadesQueen();
            player.AddCardToHand(stagecoach);
            player.PlayCard(stagecoach);

            gamePlay.GetTopCardFromDiscarded().Should().Be(stagecoach);
        }
        
        [Fact]
        public void Stagecoard_card_adds_two_cards_to_hand_and_drops_itself()
        {
            var player = CreateGameAndReturnPlayer();
            
            var stagecoachCard = new StagecoachCardType().DiamondsThree();
            
            player.AddCardToHand(stagecoachCard);

            var handSize = player.Hand.Count;
            
            player.PlayCard(stagecoachCard);

            player.Hand.Count.Should().Be(handSize + 2 - 1);
        }
        
        [Fact]
        public void WellsFargo_card_adds_three_cards_to_hand_and_drops_itself()
        {
            var player = CreateGameAndReturnPlayer();
            
            var handSize = player.Hand.Count;
            
            var wellsFargoCard = new WellsFargoCardType().DiamondsThree();
            
            player.AddCardToHand(wellsFargoCard);
            
            player.PlayCard(wellsFargoCard);

            player.Hand.Count.Should().Be(handSize + 3);
        }
        
        [Fact]
        public void Scope_card_adds_to_tablet()
        {
            var player = CreateGameAndReturnPlayer();
            var scopeCard = new ScopeCardType().SpadesQueen();
            
            player.AddCardToHand(scopeCard);
            
            // Act
            player.PlayCard(scopeCard);

            // Assert
            player.PlayerTablet.ActiveCards.Should().Contain(scopeCard);
        }
        
        [Fact]
        public void Mustang_card_adds_to_tablet()
        {
            var player = CreateGameAndReturnPlayer();
            var mustangCard = new MustangCardType().SpadesQueen();
            
            player.AddCardToHand(mustangCard);
            
            // Act
            player.PlayCard(mustangCard);

            player.PlayerTablet.ActiveCards.Should().Contain(mustangCard);
        }
        
        [Fact]
        public void Long_term_card_message_cannot_add_second_mustang_card_to_tablet()
        {
            var player = CreateGameAndReturnPlayer();
            var firstMustangCard = new MustangCardType().ClubsSeven();
            var secondMustangCard = new MustangCardType().DiamondsThree();

            player.AddCardToHand(firstMustangCard);
            player.AddCardToHand(secondMustangCard);
            
            player.PlayCard(firstMustangCard);
            
            // Act
            player.PlayCard(secondMustangCard);

            Assert.Single(player.ActiveCards);
            player.Hand.Should().Contain(secondMustangCard);
        }
        
        [Fact]
        public void Weapon_card_changes_weapon()
        {
            var player = CreateGameAndReturnPlayer();
            
            var card = new VolcanicCardType().HeartsAce();
            Weapon weapon = card.Accept(new CardToWeaponVisitor());
            
            player.AddCardToHand(card);
            player.PlayCard(card);

            player.PlayerTablet.Weapon.Should().Be(weapon);
        }

        private Player CreateGameAndReturnPlayer()
        {
            var gamePlay = InitGame();
            return gamePlay.PlayerTurn;
        }

        private Game.Gameplay InitGame()
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }
            
            var gameplay = new Game.Gameplay(CharactersDeck(), BangGameDeck());
            gameplay.Initialize(players);
            return gameplay;
        }
    }
}