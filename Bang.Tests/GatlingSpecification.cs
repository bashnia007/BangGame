using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class GatlingSpecification
    {
        #region Tests
        
        [Fact]
        public void After_played_gatling_card_goes_to_discard_deck()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());

            // Assert
            gameplay.PeekTopCardFromDiscarded().Should().Be(GatlingCard());
        }

        [Fact]
        public void Player_discards_gatling_card_after_it_played()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());

            // Assert
            actor.Hand.Should().NotContain(GatlingCard());
        }

        [Fact]
        public void If_one_victim_played_miss_card_it_goes_to_discard_deck()
        {
            var gameplay = InitGameplayWithoutCharacter(new Jourdonnais());
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());
            victim.Defense(MissedCard());

            // Assert
            gameplay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void If_one_victim_played_miss_card_it_discarded_from_hand()
        {
            var gameplay = InitGameplayWithoutCharacter(new Jourdonnais());
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());
            victim.Defense(MissedCard());

            // Assert
            victim.Hand.Should().NotContain(MissedCard());
        }

        [Fact]
        public void If_one_victim_neither_play_miss_nor_has_barrel_he_loose_life()
        {
            var gameplay = InitGameplayWithoutCharacter(new Jourdonnais());
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            int healthBefore = victim.LifePoints;

            // act
            actor.PlayCard(GatlingCard());
            victim.NotDefense();

            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void If_one_victim_does_barrel_then_he_will_not_lose_life_point()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());

            var gameplay = InitGameplay(deck);
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            var barrelCard = new BarrelCardType().SpadesQueen();
            victim.AddCardToHand(barrelCard);
            victim.PlayCard(barrelCard);
            actor.AddCardToHand(GatlingCard());

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            actor.PlayCard(GatlingCard());

            // Assert
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Players_trying_to_cancel_the_Slab_the_Killers_bang_need_to_play_one_missed_card()
        {
            var slabTheKillerCharacter = new SlabTheKiller();
            var gameplay = InitGameplayWithCharacter(slabTheKillerCharacter);
            gameplay.SetTurnToCharacter(slabTheKillerCharacter);
            (Player slabTheKiller, Player victim) = ChoosePlayer(gameplay);

            var anotherMissedCard = new MissedCardType().HeartsAce();

            var healthBefore = victim.PlayerTablet.Health;

            victim.AddCardToHand(anotherMissedCard);
            slabTheKiller.PlayCard(GatlingCard());

            // Act
            victim.Defense(MissedCard());

            // Assert
            victim.LifePoints.Should().Be(healthBefore);
            victim.Hand.Should().NotContain(MissedCard());
        }

        #endregion

        #region Private methods

        private (Player, Player) ChoosePlayer(Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(GatlingCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.AddCardToHand(MissedCard());

            return (actor, victim);
        }

        private BangGameCard GatlingCard() => new GatlingCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();

        #endregion
    }
}
