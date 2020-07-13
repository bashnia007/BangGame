using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bang.Tests.Characters
{
    public class BlackJackSpecification
    {
        private readonly ITestOutputHelper output;
        
        public BlackJackSpecification(ITestOutputHelper output)
        {
            this.output = output;
        }

        #region Tests

        [Fact]
        public void BlackJack_receives_three_cards_if_the_second_is_heart()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().DiamondsThree());
            deck.Put(new StagecoachCardType().HeartsAce());
            deck.Put(new MissedCardType().ClubsSeven());

            var (gamePlay, blackJack) = CreateGameplay(new BlackJack(), deck); 
                
            gamePlay.GivePhaseOneCards();

            blackJack.Hand.Should().HaveCount(3);
        }

        [Fact]
        public void BlackJack_receives_three_cards_if_the_second_is_diamond()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().HeartsAce());
            deck.Put(new StagecoachCardType().DiamondsThree());
            deck.Put(new MissedCardType().ClubsSeven());

            var (gamePlay, blackJack) = CreateGameplay(new BlackJack(), deck);

            gamePlay.GivePhaseOneCards();

            blackJack.Hand.Should().HaveCount(3);
        }

        [Fact]
        public void BlackJack_receives_two_cards_if_the_second_is_neither_hearts_nor_diamond()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().HeartsAce());
            deck.Put(new StagecoachCardType().ClubsSeven());
            deck.Put(new MissedCardType().DiamondsThree ());

            var (gamePlay, blackJack) = CreateGameplay(new BlackJack(), deck);

            gamePlay.GivePhaseOneCards();

            blackJack.Hand.Should().HaveCount(2);
        }

        [Fact]
        public void Not_BlackJack_receives_two_cards()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().DiamondsThree());
            deck.Put(new StagecoachCardType().HeartsAce());
            deck.Put(new MissedCardType().ClubsSeven());

            var (gamePlay, nonBlackJack) = CreateGameplay(new SlabTheKiller(), deck);

            gamePlay.GivePhaseOneCards();

            nonBlackJack.Hand.Should().HaveCount(2);
        }

        #endregion

        private (Gameplay, Player) CreateGameplay(Character character, Deck<BangGameCard> deck)
        {
            var gamePlay = 
                new GameplayBuilder()
                    .WithCharacter(character)
                    .WithDeck(deck).Build();

            var actor = gamePlay.SetTurnToCharacter(character);

            return (gamePlay, actor);
        }
    }
}
