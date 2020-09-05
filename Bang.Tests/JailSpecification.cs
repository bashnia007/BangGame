using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using System.Linq;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class JailSpecification
    {
        #region Tests

        [Fact]
        public void Player_discards_jail_card_after_it_played()
        {
            var gameplay = InitJailTestGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayJail(gameplay, victim);

            // Assert
            actor.Hand.Should().NotContain(JailCard());
        }

        [Fact]
        public void Victim_has_jail_card_on_the_tablet()
        {
            var gameplay = InitJailTestGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayJail(gameplay, victim);

            // Assert
            victim.ActiveCards.Should().Contain(JailCard());
        }

        [Fact]
        public void Jail_card_cannot_be_played_on_sheriff()
        {
            var gameplay = InitJailTestGameplay();
            (Player sheriff, Player badGuy) = ChoosePlayer(gameplay);
            badGuy.AddCardToHand(JailCard());

            // act
            badGuy.PlayJail(gameplay, sheriff);

            // Assert
            sheriff.ActiveCards.Should().NotContain(JailCard());
        }

        [Fact]
        public void Player_with_jail_card_drops_it_on_his_turn()
        {
            var gameplay = InitJailTestGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayJail(gameplay, victim);
            actor.EndTurn();

            gameplay.SetTurnToPlayer(victim);
            gameplay.PutCardOnDeck(NotHeartsCard());

            gameplay.StartPlayerTurn();

            // Assert
            victim.ActiveCards.Should().NotContain(JailCard());
        }

        [Fact]
        public void After_hearts_card_player_does_not_miss_his_turn()
        {
            var gameplay = InitJailTestGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayJail(gameplay, victim);
            actor.EndTurn();
            gameplay.SetTurnToPlayer(victim);

            gameplay.PutCardOnDeck(HeartsCard());
            gameplay.StartPlayerTurn();
            

            // Assert
            gameplay.PlayerTurn.Should().Be(victim);
        }

        [Fact]
        public void After_not_hearts_card_player_does_not_miss_his_turn()
        {
            var gameplay = InitJailTestGameplay();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayJail(gameplay, victim);
            actor.EndTurn();

            gameplay.SetTurnToPlayer(victim);

            gameplay.PutCardOnDeck(NotHeartsCard());
            gameplay.StartPlayerTurn();

            // Assert
            gameplay.PlayerTurn.Should().NotBe(victim);
        }

        #endregion

        #region Private methods

        private (Player, Player) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(JailCard());

            var victim = gameplay.Players.First(p => p != actor && !(p.Role is Sheriff));

            return (actor, victim);
        }

        private BangGameCard JailCard() => new JailCardType().SpadesQueen();

        private BangGameCard HeartsCard() => new BangCardType().HeartsAce();

        private BangGameCard NotHeartsCard() => new BangCardType().SpadesQueen();

        private Gameplay InitJailTestGameplay()
        {
            var builder = new GameplayBuilder()
                .WithoutCharacter(new KitCarlson())
                .WithoutCharacter(new PedroRamirez())
                .WithoutCharacter(new LuckyDuke());

            return builder.Build();
        }

        #endregion
    }
}
