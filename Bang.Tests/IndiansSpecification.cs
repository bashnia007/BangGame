using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System.Linq;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class IndiansSpecification
    {
        #region Tests

        [Fact]
        public void After_played_indians_card_goes_to_discard_deck()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayIndians(gameplay);

            // Assert
            gameplay.PeekTopCardFromDiscarded().Should().Be(IndiansCard());
        }

        [Fact]
        public void Player_discards_indians_card_after_it_played()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayIndians(gameplay);

            // Assert
            actor.Hand.Should().NotContain(IndiansCard());
        }

        [Fact]
        public void If_one_victim_played_bang_card_it_goes_to_discard_deck()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayIndians(gameplay);
            victim.DefenseAgainstBang(gameplay, BangCard());

            // Assert
            gameplay.PeekTopCardFromDiscarded().Should().Be(BangCard());
        }

        [Fact]
        public void If_one_victim_played_bang_card_it_discarded_from_hand()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayIndians(gameplay);
            victim.DefenseAgainstBang(gameplay, BangCard());

            // Assert
            victim.Hand.Should().NotContain(BangCard());
        }

        [Fact]
        public void If_one_victim_does_not_play_bang_he_loses_life()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            int healthBefore = victim.LifePoints;

            // act
            actor.PlayIndians(gameplay);
            victim.NotDefenseAgainstBang(gameplay);

            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void If_one_victim_plays_then_he_will_not_lose_life_point()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);
            var stageCoach = new StagecoachCardType().HeartsAce();

            var healthBefore = victim.PlayerTablet.Health;
            gameplay.PutCardOnDeck(stageCoach);

            // Act
            actor.PlayIndians(gameplay);
            victim.DefenseAgainstBang(gameplay, BangCard());

            // Assert
            victim.LifePoints.Should().Be(healthBefore);
        }

        #endregion

        #region Private methods

        private (Player, Player) ChoosePlayer(Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(IndiansCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.AddCardToHand(BangCard());

            return (actor, victim);
        }

        private BangGameCard IndiansCard() => new IndiansCardType().SpadesQueen();
        private BangGameCard BangCard() => new BangCardType().SpadesQueen();

        #endregion
    }
}
