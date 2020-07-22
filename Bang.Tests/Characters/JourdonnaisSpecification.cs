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
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());

            var gamePlay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<Done>();
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Jourdonnais_can_use_second_barrel_if_default_barrel_did_not_help()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new MissedCardType().ClubsSeven());
            deck.Put(new StagecoachCardType().HeartsAce());

            var gamePlay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var barrelCard = new BarrelCardType().SpadesQueen();
            victim.AddCardToHand(barrelCard);
            victim.PlayCard(barrelCard);

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<Done>();
            victim.LifePoints.Should().Be(healthBefore);
        }

        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();

        private Gameplay InitGame(Deck<BangGameCard> deck = null)
        {
            var builder = new GameplayBuilder()
                .WithCharacter(new Jourdonnais())
                .WithoutCharacter(new SlabTheKiller());

            if (deck != null)
                builder.WithDeck(deck);

            return builder.Build();
        }

        private (Player actor, Player victim) ChoosePlayers(Gameplay gameplay)
        {
            var victim = gameplay.FindPlayer(new Jourdonnais());

            gameplay.SkipTurnsUntilPlayer(victim);
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(BangCard());
            
            Debug.Assert(victim != actor, "player can't bang to himself!");

            return (actor, victim);
        }

        #endregion
    }
}