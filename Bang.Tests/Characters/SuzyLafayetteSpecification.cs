using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System.Linq;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests.Characters
{
    public class SuzyLafayetteSpecification
    {
        #region Tests
        
        [Fact]
        public void SuzyLafayette_receives_a_new_card_when_played_the_last_one_during_her_turn()
        {
            var (gamePlay, suzy) = CreateGameplayWithSuzy();
            suzy.AddCardToHand(BangCard);
            var victim = gamePlay.Players.First(p => p != suzy);

            suzy.PlayCard(BangCard, victim);

            suzy.Hand.Count.Should().Be(1);
        }

        [Fact]
        public void SuzyLafayette_receives_a_new_card_when_defense_from_bang()
        {
            var (gamePlay, suzy) = CreateGameplayWithSuzy();
            suzy.AddCardToHand(MissedCard);
            var attacker = gamePlay.Players.First(p => p != suzy);
            attacker.AddCardToHand(BangCard);

            attacker.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.Hand.Count.Should().Be(1);
        }

        [Fact]
        public void SuzyLafayette_can_receive_a_new_card_only_after_duel()
        {
            var (gamePlay, suzy) = CreateGameplayWithSuzy();
            suzy.AddCardToHand(DuelCard);
            var opponent = gamePlay.Players.First(p => p != suzy);
            opponent.AddCardToHand(BangCard);
            var suzyLife = suzy.LifePoints;

            suzy.PlayCard(DuelCard, opponent);
            suzy.Hand.Count.Should().Be(0);
            opponent.Defense(BangCard);
            suzy.NotDefense();

            suzy.LifePoints.Should().Be(suzyLife - 1);
            suzy.Hand.Count.Should().Be(1);
        }

        [Fact]
        public void SuzyLafayette_uses_missed_card_on_the_deck_against_Slab_when_plays_the_last_card()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(SecondMissedCard);

            var suzyLife = suzy.LifePoints;

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.LifePoints.Should().Be(suzyLife);
            gameplay.PeekTopCardFromDeck().Should().NotBe(SecondMissedCard);
        }

        [Fact]
        public void SuzyLafayette_drops_missed_card_against_Slab_when_plays_the_last_card()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(SecondMissedCard);

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.Hand.Should().NotContain(MissedCard);
            suzy.Hand.Should().NotContain(SecondMissedCard);
        }

        [Fact]
        public void SuzyLafayette_at_the_end_receives_additional_card_against_Slab_when_plays_the_last_card()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(SecondMissedCard);

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.Hand.Count.Should().Be(1);
        }

        [Fact]
        public void SuzyLafayette_takes_not_missed_card_to_hand_against_Slab_when_plays_the_last_card()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(DuelCard);

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.Hand.Count.Should().Be(1);
            suzy.Hand.Should().Contain(DuelCard);
        }

        [Fact]
        public void SuzyLafayette_drops_missed_card_against_Slab_when_plays_last_card_and_did_not_saved()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(DuelCard);

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.Hand.Should().NotContain(MissedCard);
        }

        [Fact]
        public void SuzyLafayette_loses_life_against_Slab_when_plays_last_card_and_did_not_saved()
        {
            var (gameplay, slab, suzy) = CreateGameplayWithSlabTheKillerAndSuzy();
            suzy.AddCardToHand(MissedCard);
            slab.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(DuelCard);

            var suzyLife = suzy.LifePoints;

            slab.PlayCard(BangCard, suzy);
            suzy.Defense(MissedCard);

            suzy.LifePoints.Should().Be(suzyLife - 1);
        }

        [Fact]
        public void ElGringo_draws_taken_by_Suzy_card_when_she_hits_him_with_the_last_card()
        {
            var (gameplay, suzy, el) = CreateGameplayWithElGringoAndSuzy();
            suzy.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(MissedCard);

            suzy.PlayCard(BangCard, el);
            el.NotDefense();

            el.Hand.Should().Contain(MissedCard);
        }

        [Fact]
        public void Suzy_receives_additional_card_when_she_hits_ElGringo_with_the_last_card()
        {
            var (gameplay, suzy, el) = CreateGameplayWithElGringoAndSuzy();
            suzy.AddCardToHand(BangCard);
            gameplay.PutCardOnDeck(MissedCard);
            gameplay.PutCardOnDeck(DuelCard);

            suzy.PlayCard(BangCard, el);
            el.NotDefense();

            suzy.Hand.Count.Should().Be(1);
            suzy.Hand.Should().Contain(MissedCard);
        }

        #endregion

        readonly BangGameCard BangCard = new BangCardType().ClubsFive();
        readonly BangGameCard MissedCard = new MissedCardType().ClubsFive();
        readonly BangGameCard SecondMissedCard = new MissedCardType().ClubsSeven();
        readonly BangGameCard DuelCard = new DuelCardType().ClubsFive();

        private (Gameplay, Player) CreateGameplayWithSuzy()
        {
            var gamePlay = InitGameplay();
            var suzy = gamePlay.PlayerTurn;
            suzy.SetInfo(gamePlay, suzy.Role, new SuzyLafayette());

            return (gamePlay, suzy);
        }

        private (Gameplay, Player, Player) CreateGameplayWithSlabTheKillerAndSuzy()
        {
            var gamePlay = InitGameplay();
            var slabTheKiller = gamePlay.PlayerTurn;
            slabTheKiller.SetInfo(gamePlay, slabTheKiller.Role, new SlabTheKiller());
            var suzy = gamePlay.Players.First(p => p != slabTheKiller);
            suzy.SetInfo(gamePlay, suzy.Role, new SuzyLafayette());

            return (gamePlay, slabTheKiller, suzy);
        }

        private (Gameplay, Player, Player) CreateGameplayWithElGringoAndSuzy()
        {
            var gamePlay = InitGameplay();
            var suzy = gamePlay.PlayerTurn;
            suzy.SetInfo(gamePlay, suzy.Role, new SuzyLafayette());
            var el = gamePlay.Players.First(p => p != suzy);
            el.SetInfo(gamePlay, el.Role, new ElGringo());

            return (gamePlay, suzy, el);
        }
    }
}
