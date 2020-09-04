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
        #region Tests

        [Fact]
        public void BlackJack_receives_three_cards_if_the_second_is_heart()
        {
            var (gamePlay, blackJack) = CreateGameplay(new BlackJack());
            
            gamePlay.PutCardOnDeck(new BangCardType().DiamondsThree());
            gamePlay.PutCardOnDeck(new StagecoachCardType().HeartsAce());
            gamePlay.PutCardOnDeck(new MissedCardType().ClubsSeven());

            gamePlay.StartPlayerTurn();

            gamePlay.PlayerTurn.Should().Be(blackJack);
            blackJack.Hand.Should().HaveCount(3);
        }

        [Fact]
        public void BlackJack_receives_three_cards_if_the_second_is_diamond()
        {
            var (gamePlay, blackJack) = CreateGameplay(new BlackJack());
            
            gamePlay.PutCardOnDeck(new BangCardType().HeartsAce());
            gamePlay.PutCardOnDeck(new StagecoachCardType().DiamondsThree());
            gamePlay.PutCardOnDeck(new MissedCardType().ClubsSeven());

            gamePlay.StartPlayerTurn();

            blackJack.Hand.Should().HaveCount(3);
        }

        [Fact]
        public void BlackJack_receives_two_cards_if_the_second_is_neither_hearts_nor_diamond()
        {
            var (gamePlay, blackJack) = CreateGameplay(new BlackJack());
            
            gamePlay.PutCardOnDeck(new BangCardType().HeartsAce());
            gamePlay.PutCardOnDeck(new StagecoachCardType().ClubsSeven());
            gamePlay.PutCardOnDeck(new MissedCardType().DiamondsThree());

            gamePlay.StartPlayerTurn();

            gamePlay.PlayerTurn.Should().Be(blackJack);
            blackJack.Hand.Should().HaveCount(2);
        }

        [Fact]
        public void Not_BlackJack_receives_two_cards()
        {
            var (gamePlay, nonBlackJack) = CreateGameplay(new SlabTheKiller());
            
            gamePlay.PutCardOnDeck(new BangCardType().DiamondsThree());
            gamePlay.PutCardOnDeck(new StagecoachCardType().HeartsAce());
            gamePlay.PutCardOnDeck(new MissedCardType().ClubsSeven());

            gamePlay.StartPlayerTurn();

            nonBlackJack.Hand.Should().HaveCount(2);
        }

        #endregion

        private (Gameplay, Player) CreateGameplay(Character character)
        {
            var gamePlay = 
                new GameplayBuilder()
                    .WithCharacter(character)
                    .Build();

            var actor = gamePlay.SetTurnToCharacter(character);

            return (gamePlay, actor);
        }
    }
}
