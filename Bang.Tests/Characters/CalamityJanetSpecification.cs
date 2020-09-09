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

            calamity.PlayMissed(gamePlay, opponent);

            var healthBefore = opponent.PlayerTablet.Health;
            // Act
            opponent.NotDefenseAgainstBang(gamePlay);

            // Assert
            opponent.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Bang_As_Usual_When_Shoots()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            calamity.PlayBang(gamePlay, opponent);

            var healthBefore = opponent.PlayerTablet.Health;
            // Act
            opponent.NotDefenseAgainstBang(gamePlay);

            // Assert
            opponent.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Bang_As_Missed_When_Defends()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            gamePlay.SetTurnToPlayer(opponent);
            opponent.PlayBang(gamePlay, calamity);

            // Act
            calamity.DefenseAgainstBang(gamePlay, BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Calamity_Janet_Can_Play_Missed_As_Usual_When_Defends()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;

            gamePlay.SetTurnToPlayer(opponent);
            opponent.PlayBang(gamePlay, calamity);

            // Act
            calamity.DefenseAgainstBang(gamePlay, MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayDuel(gamePlay, calamity);
            calamity.DefenseAgainstBang(gamePlay, MissedCard());
            opponent.NotDefenseAgainstBang(gamePlay);

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_As_Usual_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayDuel(gamePlay, calamity);
            calamity.DefenseAgainstBang(gamePlay, BangCard());
            opponent.NotDefenseAgainstBang(gamePlay);

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayIndians(gamePlay);
            calamity.DefenseAgainstBang(gamePlay, MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayIndians(gamePlay);
            calamity.DefenseAgainstBang(gamePlay, BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Bang_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            gamePlay.SetTurnToPlayer(opponent);
            var healthBefore = calamity.PlayerTablet.Health;

            // Act
            opponent.PlayGatling(gamePlay);
            calamity.DefenseAgainstBang(gamePlay, BangCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Can_Play_Missed_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            var healthBefore = calamity.PlayerTablet.Health;
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayGatling(gamePlay);
            calamity.DefenseAgainstBang(gamePlay, MissedCard());

            // Assert
            calamity.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Plays_It_As_Bang()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            calamity.PlayMissed(gamePlay, opponent);

            // Act
            opponent.NotDefenseAgainstBang(gamePlay);

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Bang_Card_When_Plays_It_As_Missed()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            gamePlay.SetTurnToPlayer(opponent);
            opponent.PlayBang(gamePlay, calamity);

            // Act
            calamity.DefenseAgainstBang(gamePlay, BangCard());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(BangCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Plays_It_During_Duel()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);

            // Act
            opponent.PlayDuel(gamePlay, calamity);
            calamity.DefenseAgainstDuel(gamePlay, MissedCard());
            opponent.LoseDuel(gamePlay);

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Missed_Card_When_Plays_It_Against_Indians()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);
            
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayIndians(gamePlay);
            calamity.DefenseAgainstDuel(gamePlay, MissedCard());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void Camility_Janet_Drops_Bang_Card_When_Plays_It_Against_Gatling()
        {
            var gamePlay = InitGameplayWithCharacter(new CalamityJanet());
            (Player calamity, Player opponent) = ChoosePlayers(gamePlay);
            
            gamePlay.SetTurnToPlayer(opponent);

            // Act
            opponent.PlayGatling(gamePlay);
            calamity.DefenseAgainstBang(gamePlay, BangCard());

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

            var opponent = gameplay.FindPlayerAtDistanceFrom(1, calamity);
            opponent.AddCardToHand(BangCard()); 
            opponent.AddCardToHand(GatlingCard());
            opponent.AddCardToHand(IndiansCard());
            opponent.AddCardToHand(DuelCard());

            return (calamity, opponent);
        }

        #endregion
    }
}
