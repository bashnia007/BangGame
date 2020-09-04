using System.Diagnostics;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

namespace Bang.Tests.Characters
{
    public class JourdonnaisSpecification
    {
        #region Tests

        [Fact]
        public void Jourdonnais_starts_game_with_4_life_points()
        {
            var gamePlay = InitGame();
            var jourdonnais = gamePlay.FindPlayer(new Jourdonnais());
            jourdonnais.AsOutlaw(gamePlay);

            jourdonnais.LifePoints.Should().Be(4);
        }

        [Fact]
        public void Jourdonnais_checks_default_barrel_when_attacked()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var healthBefore = victim.PlayerTablet.Health;
            
            gamePlay.PutCardOnDeck(new StagecoachCardType().HeartsAce());
            // Act
            var response = actor.PlayBang(gamePlay, victim);

            // Assert
            response.Should().BeOfType<Done>();
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Jourdonnais_can_use_second_barrel_if_default_barrel_did_not_help()
        {
            var gamePlay = InitGame();
            (Player actor, Player jourdonnais) = ChoosePlayers(gamePlay);

            var barrelCard = new BarrelCardType().SpadesQueen();
            jourdonnais.PlayerTablet.PutCard(barrelCard);

            var healthBefore = jourdonnais.PlayerTablet.Health;

            gamePlay.PutCardOnDeck(new StagecoachCardType().HeartsAce());
            gamePlay.PutCardOnDeck(new MissedCardType().ClubsSeven());
            // Act
            var response = actor.PlayBang(gamePlay, jourdonnais);

            // Assert
            response.Should().BeOfType<Done>();
            jourdonnais.LifePoints.Should().Be(healthBefore);
        }

        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();

        private Gameplay InitGame()
        {
            var builder = new GameplayBuilder()
                .WithCharacter(new Jourdonnais())
                .WithoutCharacter(new SlabTheKiller());

            return builder.Build();
        }

        private (Player actor, Player victim) ChoosePlayers(Gameplay gameplay)
        {
            var jourdonnais = gameplay.FindPlayer(new Jourdonnais());

            var actor = gameplay.FindPlayerAtDistanceFrom(1, jourdonnais);
            actor.AddCardToHand(BangCard());

            gameplay.SetTurnToPlayer(actor);
            gameplay.StartPlayerTurn();

            Debug.Assert(jourdonnais != actor, "player can't bang to himself!");

            return (actor, jourdonnais);
        }

        #endregion
    }
}