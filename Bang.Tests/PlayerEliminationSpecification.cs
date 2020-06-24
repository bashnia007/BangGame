using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class PlayerEliminationSpecification
    {
        [Fact]
        public void if_the_sheriff_eliminates_a_deputy_the_sheriff_must_discard_all_the_cards()
        {
            var gamePlay = InitGame();
            var sheriff = gamePlay.Players.First(p => p.Role is Sheriff);
            sheriff.AddCardToHand(new SaloonCardType().ClubsSeven());
            sheriff.PlayerTablet.PutCard(new ScopeCardType().HeartsAce());
            
            var deputy = gamePlay.Players.First(p => p.Role is Deputy);
            
            // Act
            deputy.LoseLifePoint(sheriff, deputy.MaximumLifePoints);
            
            // Assert
            sheriff.Hand.Should().BeEmpty();
            sheriff.ActiveCards.Should().BeEmpty();
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.AllRoles), MemberType = typeof(TestDataGenerator))]
        public void Any_player_eliminating_an_outlaw_must_draw_a_reward_of_3_cards_from_the_deck(Role killerRole)
        {
            var gamePlay = InitGame();
            var killer = gamePlay.Players.First(p => p.Role == killerRole);
            
            var outLaw = gamePlay.Players.First(p => p.Role is Outlaw && p != killer);
            
            // Act
            outLaw.LoseLifePoint(killer, outLaw.MaximumLifePoints);
            
            // Assert
            killer.Hand.Count.Should().Be(3);
        }

        [Fact]
        public void Hand_cards_of_eliminated_player_are_discarded()
        {
            var gamePlay = InitGame();
            var victim = gamePlay.Players.First();

            var catBalouCard = new CatBalouCardType().HeartsAce();
            victim.AddCardToHand(catBalouCard);
            
            // Act
            victim.LoseLifePoint(null, victim.PlayerTablet.MaximumHealth);
            
            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(catBalouCard);
        }
        
        [Fact]
        public void Active_cards_of_eliminated_player_are_discarded()
        {
            var gamePlay = InitGame();
            var victim = gamePlay.Players.First();

            var scopeCard = new ScopeCardType().HeartsAce();
            victim.PlayerTablet.PutCard(scopeCard);
            
            // Act
            victim.LoseLifePoint(null, victim.MaximumLifePoints);
            
            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(scopeCard);
        }
        
        private Game.Gameplay InitGame() => InitGame(BangGameDeck());

        private Game.Gameplay InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }

            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);

            return gameplay;
        }
    }
}