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
        public void If_the_sheriff_eliminates_a_deputy_the_sheriff_must_discard_all_the_cards()
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

        [Fact]
        public void If_an_outlaw_plays_a_duel_and_loses_then_no_one_will_gain_a_reward()
        {
            var gameplay = InitGame();
            var outlaw = gameplay.Players.First(p => p.Role is Outlaw);
            outlaw.LoseLifePoint(outlaw.MaximumLifePoints - 1);
            
            var duelCard = new DuelCardType().ClubsSeven();
            outlaw.AddCardToHand(duelCard);
            
            while (gameplay.GetNextPlayer() != outlaw)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();

            var otherPlayer = gameplay.Players.First(p => p != outlaw);
            var bangCard = new BangCardType().SpadesQueen();
            otherPlayer.AddCardToHand(bangCard);
            
            // Act
            outlaw.PlayCard(duelCard, otherPlayer);
            otherPlayer.Defense(bangCard);
            outlaw.NotDefense();
            
            // Assert
            otherPlayer.Hand.Should().BeEmpty();
        }
        
        [Fact]
        public void If_a_deputy_plays_a_duel_to_sheriff_and_dies_then_sheriff_will_not_drop_cards()
        {
            var gameplay = InitGame();
            var deputy = gameplay.Players.First(p => p.Role is Deputy);
            deputy.LoseLifePoint(deputy.MaximumLifePoints - 1);
            
            var duelCard = new DuelCardType().ClubsSeven();
            deputy.AddCardToHand(duelCard);
            
            while (gameplay.GetNextPlayer() != deputy)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();

            var sheriff = gameplay.Players.First(p => p.Role is Sheriff);
            var bangCard = new BangCardType().SpadesQueen();
            sheriff.AddCardToHand(bangCard);
            sheriff.AddCardToHand(new CatBalouCardType().DiamondsThree());
            sheriff.PlayerTablet.PutCard(new ScopeCardType().SpadesQueen());
            
            // Act
            deputy.PlayCard(duelCard, sheriff);
            sheriff.Defense(bangCard);
            deputy.NotDefense();
            
            // Assert
            sheriff.Hand.Should().NotBeEmpty();
            sheriff.ActiveCards.Should().NotBeEmpty();
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