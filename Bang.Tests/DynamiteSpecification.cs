using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class DynamiteSpecification
    {
        #region Tests

        [Fact]
        public void Player_discards_dynamite_card_after_it_played()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(DynamiteCard());

            // Assert
            actor.Hand.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_has_dynamite_card_on_his_tablet_after_it_played()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(DynamiteCard());

            // Assert
            actor.PlayerTablet.ActiveCards.Should().Contain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_loses_3_life_points_if_dynamite_explodes()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(ExplodeCard());

            var gameplay = InitGame(deck);
            Player actor = ChoosePlayer(gameplay);
            int healthBefore = actor.LifePoints;

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());
            while (gameplay.GetNextPlayer() != actor)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();

            // Assert
            actor.LifePoints.Should().Equals(healthBefore - 3);
        }

        [Fact]
        public void When_dynamite_explodes_it_leaves_player_tablet()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(ExplodeCard());

            var gameplay = InitGame(deck);
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());
            while (gameplay.GetNextPlayer() != actor)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();

            // Assert
            actor.ActiveCards.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_transfers_dynamite_card_if_dynamite_doesnot_explode()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(NotExplodeCard());

            var gameplay = InitGame(deck);
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());
            while (gameplay.GetNextPlayer() != actor)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();

            // Assert
            actor.ActiveCards.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_with_dynamite_card_transfers_dynamite_card_to_the_next_player_if_dynamite_doesnot_explode()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(NotExplodeCard());

            var gameplay = InitGame(deck);
            Player actor = ChoosePlayer(gameplay);

            // act
            actor.PlayerTablet.PutCard(DynamiteCard());
            while (gameplay.GetNextPlayer() != actor)
                gameplay.SetNextPlayer();

            gameplay.StartNextPlayerTurn();
            gameplay.SetNextPlayer();

            // Assert
            gameplay.PlayerTurn.ActiveCards.Should().Contain(DynamiteCard());
        }

        #endregion

        #region Private methods

        private Game.Gameplay InitGame() => InitGame(BangGameDeck());

        private Game.Gameplay InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }

            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);
            return gameplay;
        }

        private Player ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(DynamiteCard());

            return actor;
        }

        private BangGameCard DynamiteCard() => new DynamiteCardType().SpadesQueen();

        private BangGameCard ExplodeCard() => new DynamiteCardType().ClubsSeven();

        private BangGameCard NotExplodeCard() => new DynamiteCardType().SpadesQueen();

        #endregion
    }
}
