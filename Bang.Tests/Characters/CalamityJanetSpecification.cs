using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests.Characters
{
    public class CalamityJanetSpecification
    {
        #region Tests
        
        [Fact]
        public void Calamity_Janet_Can_Play_Missed_As_Bang_When_Shoots()
        {
            var gamePlay = 
                new GameplayBuilder()
                    .WithCharacter(new CalamityJanet())
                    .WithoutCharacter(new Jourdonnais())
                    .Build();
            
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            calamity.PlayCard(MissedCard(), opponent);

            var healthBefore = opponent.PlayerTablet.Health;
            // Act
            opponent.NotDefense();

            // Assert
            opponent.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Bang_As_Usual_When_Shoots()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            calamity.PlayCard(BangCard(), opponent);

            var healthBefore = opponent.PlayerTablet.Health;
            // Act
            opponent.NotDefense();

            // Assert
            opponent.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Bang_As_Missed_When_Defends()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            opponent.PlayCard(BangCard(), calamity);

            // Act
            calamity.Defense(BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Missed_As_Usual_When_Defends()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            opponent.PlayCard(BangCard(), calamity);

            // Act
            calamity.Defense(MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(DuelCard(), calamity);
            calamity.Defense(MissedCard());
            opponent.NotDefense();

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_As_Usual_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(DuelCard(), calamity);
            calamity.Defense(BangCard());
            opponent.NotDefense();

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(IndiansCard());
            calamity.Defense(MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(IndiansCard());
            calamity.Defense(BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(GatlingCard());
            calamity.Defense(BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayCard(GatlingCard());
            calamity.Defense(MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Playes_It_As_Bang()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            calamity.PlayCard(MissedCard(), opponent);

            // Act
            opponent.NotDefense();

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Bang_Card_When_Playes_It_As_Missed()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            opponent.PlayCard(BangCard(), calamity);

            // Act
            calamity.Defense(BangCard());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(BangCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Playes_It_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            // Act
            opponent.PlayCard(DuelCard(), calamity);
            calamity.Defense(MissedCard());
            opponent.NotDefense();

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Playes_It_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            // Act
            opponent.PlayCard(IndiansCard());
            calamity.Defense(MissedCard());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Bang_Card_When_Playes_It_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            // Act
            opponent.PlayCard(GatlingCard());
            calamity.Defense(BangCard());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(BangCard());
        }


        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();
        private BangGameCard DuelCard() => new DuelCardType().SpadesQueen();
        private BangGameCard IndiansCard() => new IndiansCardType().SpadesQueen();
        private BangGameCard GatlingCard() => new GatlingCardType().SpadesQueen();

        private (Player calamity, Player victim) ChoosePlayers(Gameplay gameplay)
        {
            var calamity = gameplay.Players.First(p => p.Character == new CalamityJanet());
            gameplay.SetTurnToPlayer(calamity);
            calamity.AddCardToHand(BangCard());
            calamity.AddCardToHand(MissedCard());

            var opponent = gameplay.Players.First(p => p != calamity);
            opponent.AddCardToHand(BangCard()); 
            opponent.AddCardToHand(GatlingCard());
            opponent.AddCardToHand(IndiansCard());
            opponent.AddCardToHand(DuelCard());

            return (calamity, opponent);
        }

        #endregion
    }
}
