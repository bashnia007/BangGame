using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class IndiansSpecification
    {
        #region Tests

        [Fact]
        public void After_played_indians_card_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(IndiansCard());

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(IndiansCard());
        }

        [Fact]
        public void Player_discards_indians_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(IndiansCard());

            // Assert
            actor.Hand.Should().NotContain(IndiansCard());
        }

        [Fact]
        public void If_one_victim_played_bang_card_it_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(IndiansCard());
            victim.Defense(BangCard());

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(BangCard());
        }

        [Fact]
        public void If_one_victim_played_bang_card_it_discarded_from_hand()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(IndiansCard());
            victim.Defense(BangCard());

            // Assert
            victim.Hand.Should().NotContain(BangCard());
        }

        [Fact]
        public void If_one_victim_does_not_play_bang_he_loses_life()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            int healthBefore = victim.LifePoints;

            // act
            actor.PlayCard(IndiansCard());
            victim.NotDefense();

            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void If_one_victim_plays_then_he_will_not_lose_life_point()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());

            var gameplay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            actor.PlayCard(IndiansCard());
            victim.Defense(BangCard());

            // Assert
            victim.LifePoints.Should().Be(healthBefore);
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

        private (Player, Player) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(IndiansCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            victim.AddCardToHand(BangCard());

            return (actor, victim);
        }

        private BangGameCard IndiansCard() => new IndiansCardType().SpadesQueen();
        private BangGameCard BangCard() => new BangCardType().SpadesQueen();

        #endregion
    }
}
