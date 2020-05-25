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
    public class GatlingSpecification
    {
        #region Tests
        
        [Fact]
        public void After_played_gatling_card_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(GatlingCard());
        }

        [Fact]
        public void Player_discards_gatling_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());

            // Assert
            actor.Hand.Should().NotContain(GatlingCard());
        }

        [Fact]
        public void If_one_victim_played_miss_card_it_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(GatlingCard());
            victim.Defense(MissedCard());

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(MissedCard());
        }

        [Fact]
        public void If_one_victim_played_miss_card_it_discarded_from_hand()
        {
            var gameplay = InitGame();
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
            var gameplay = InitGame();
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

            var gameplay = InitGame(deck);
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
        public void Players_trying_to_cancel_the_Slab_the_Killers_bang_need_to_play_two_missed_cards()
        {
            var gameplay = InitGame();
            (Player slabTheKiller, Player victim) = ChoosePlayer(gameplay);

            slabTheKiller.SetInfo(gameplay, slabTheKiller.Role, new SlabTheKiller());

            var anotherMissedCard = new MissedCardType().HeartsAce();

            var healthBefore = victim.PlayerTablet.Health;

            victim.AddCardToHand(anotherMissedCard);
            slabTheKiller.PlayCard(GatlingCard());

            // Act
            victim.Defense(MissedCard(), anotherMissedCard);

            // Assert
            victim.LifePoints.Should().Be(healthBefore);
            victim.Hand.Should().NotContain(MissedCard());
            victim.Hand.Should().NotContain(anotherMissedCard);
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
            actor.AddCardToHand(GatlingCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            victim.AddCardToHand(MissedCard());

            return (actor, victim);
        }

        private BangGameCard GatlingCard() => new GatlingCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();

        #endregion
    }
}
