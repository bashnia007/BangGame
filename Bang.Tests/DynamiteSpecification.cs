using System.Diagnostics;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class DynamiteSpecification
    {
        #region Tests

        [Fact]
        public void Player_discards_dynamite_card_after_it_played()
        {
            var gameplay = InitGameplay();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayDynamite(gameplay);

            // Assert
            actor.Hand.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_has_dynamite_card_on_his_tablet_after_it_played()
        {
            var gameplay = InitGameplay();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayDynamite(gameplay);

            // Assert
            actor.PlayerTablet.ActiveCards.Should().Contain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_loses_3_life_points_if_dynamite_explodes()
        {
            var deck = new Deck<BangGameCard>();

            var gameplay = CreateGameplay(deck);
            Player actor = ChoosePlayer(gameplay);
            int healthBefore = actor.LifePoints;

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());

            gameplay.SetTurnToPlayer(actor);

            gameplay.PutCardOnDeck(ExplodeCard());
            gameplay.StartPlayerTurn();

            // Assert
            actor.LifePoints.Should().Be(healthBefore - 3);
        }

        [Fact]
        public void When_dynamite_explodes_it_leaves_player_tablet()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(ExplodeCard());

            var gameplay = CreateGameplay(deck);
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());
            
            gameplay.SetTurnToPlayer(actor);

            gameplay.StartPlayerTurn();

            // Assert
            actor.ActiveCards.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_transfers_dynamite_card_if_dynamite_doesnot_explode()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(NotExplodeCard());

            var gameplay = CreateGameplay(deck);
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());

            gameplay.SetTurnToPlayer(actor);

            gameplay.StartPlayerTurn();

            // Assert
            actor.ActiveCards.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_transfers_dynamite_card_to_the_next_player_if_dynamite_doesnot_explode()
        {
            var gameplay = InitGameplay();
            Player actor = ChoosePlayer(gameplay);

            actor.PlayerTablet.PutCard(DynamiteCard());
            actor.EndTurn();
            gameplay.SetTurnToPlayer(actor);
            
            // act
            gameplay.PutCardOnDeck(new VolcanicCardType().DiamondsFour());
            gameplay.StartPlayerTurn();
            

            // Assert
            gameplay.GetNextPlayer().ActiveCards.Should().Contain(DynamiteCard());
        }

        #endregion

        #region Private methods

        private Player ChoosePlayer(Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(DynamiteCard());

            return actor;
        }

        private BangGameCard DynamiteCard() => new DynamiteCardType().SpadesQueen();

        private BangGameCard ExplodeCard() => new DynamiteCardType().ClubsSeven();

        private BangGameCard NotExplodeCard() => new DynamiteCardType().SpadesQueen();

        private Gameplay CreateGameplay(Deck<BangGameCard> deck)
        {
            return new GameplayBuilder()
                .WithDeck(deck)
                .WithoutCharacter(new BartCassidy())
                .Build(); 
        }

        #endregion
    }
}
