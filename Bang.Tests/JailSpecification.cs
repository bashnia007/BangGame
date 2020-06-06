using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class JailSpecification
    {
        #region Tests

        [Fact]
        public void Player_discards_jail_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(JailCard(), victim);

            // Assert
            actor.Hand.Should().NotContain(JailCard());
        }

        [Fact]
        public void Victim_has_jail_card_on_the_tablet()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(JailCard(), victim);

            // Assert
            victim.ActiveCards.Should().Contain(JailCard());
        }

        [Fact]
        public void Jail_card_cannot_be_played_on_sheriff()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayer(gameplay);
            victim.AddCardToHand(JailCard());

            // act
            victim.PlayCard(JailCard(), actor);

            // Assert
            actor.ActiveCards.Should().NotContain(JailCard());
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
            actor.AddCardToHand(JailCard());

            var victim = gameplay.Players.First(p => p != actor && !(p.Role is Sheriff));
            victim.SetInfo(gameplay, victim.Role, new PedroRamirez());

            return (actor, victim);
        }

        private BangGameCard JailCard() => new JailCardType().SpadesQueen();

        private BangGameCard FreedomCard() => new BangCardType().HeartsAce();

        private BangGameCard NotFreedomCard() => new BangCardType().SpadesQueen();

        #endregion
    }
}
