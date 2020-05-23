using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Player actor = ChoosePlayer(gameplay);

            var gatlingCard = new GatlingCardType().ClubsSeven();
            actor.AddCardToHand(gatlingCard);

            // act
            actor.PlayCard(gatlingCard);

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(gatlingCard);
        }

        [Fact]
        public void If_one_victim_played_miss_it_goes_to_discard_deck()
        {
            var gameplay = InitGame();
            Player actor = ChoosePlayer(gameplay);

            var gatlingCard = new GatlingCardType().ClubsSeven();
            actor.AddCardToHand(gatlingCard);

            // act
            actor.PlayCard(gatlingCard);

            var missedCard = new MissedCardType().ClubsSeven();
            gameplay.Players[1].AddCardToHand(missedCard);
            gameplay.Players[1].Defense(missedCard);

            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(gatlingCard);
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

            return actor;
        }

        #endregion
    }
}
