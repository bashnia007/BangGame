using Bang.Characters;
using Bang.Game;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class BarrelCheckerSpecification
    {
        [Theory]
        [InlineData(Suite.Clubs, false)]
        [InlineData(Suite.Diamonds, false)]
        [InlineData(Suite.Spades, false)]
        [InlineData(Suite.Hearts, true)]
        public void Barrel_is_success_when_top_deck_card_is_heart(Suite suite, bool draw)
        {
            // Arrange
            var deck = new Deck<BangGameCard>();
            var topDeckCard = new BangGameCard(new BeerCardType(), suite, Rank.Ten); 
            deck.Put(topDeckCard);

            var gamePlay = InitGameplay(deck);
            
            var barrelChecker = new BarrelChecker();
            
            // Act
            var barrelResult = barrelChecker.Draw(gamePlay, new Jourdonnais());

            // Assert
            barrelResult.Should().Be(draw);
        }

        [Fact]
        public void Card_after_draw_is_discarded()
        {
            // Arrange
            var deck = new Deck<BangGameCard>();
            var card = new BangGameCard(new BeerCardType(), Suite.Clubs, Rank.Ten); 
            deck.Put(card);

            var gamePlay = InitGameplay(deck);
            
            var barrelChecker = new BarrelChecker();
            
            // Act
            var barrelResult = barrelChecker.Draw(gamePlay, new Jourdonnais());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(card);
        }

        [Fact]
        public void Lucky_Duke_flips_two_cards()
        {
            // Arrange
            var deck = new Deck<BangGameCard>();
            var heartCard = new BangGameCard(new BeerCardType(), Suite.Hearts, Rank.Ten); 
            var diamondCard = new BangGameCard(new StagecoachCardType(), Suite.Diamonds, Rank.Ten); 
            deck.Put(heartCard);
            deck.Put(diamondCard);

            var gamePlay = InitGameplay(deck);
            
            var barrelChecker = new BarrelChecker();
            
            // Act
            var barrelResult = barrelChecker.Draw(gamePlay, new LuckyDuke());

            // Assert
            barrelResult.Should().BeTrue();
        }
        
        [Fact]
        public void Both_cards_flipped_by_Lucky_Duke_are_discarded()
        {
            // Arrange
            var deck = new Deck<BangGameCard>();
            var heartCard = new BangGameCard(new BeerCardType(), Suite.Hearts, Rank.Ten); 
            var diamondCard = new BangGameCard(new StagecoachCardType(), Suite.Diamonds, Rank.Ten); 
            deck.Put(heartCard);
            deck.Put(diamondCard);

            var gamePlay = InitGameplay(deck);
            
            var barrelChecker = new BarrelChecker();
            
            // Act
            var barrelResult = barrelChecker.Draw(gamePlay, new LuckyDuke());

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(heartCard);
        }
    }
}