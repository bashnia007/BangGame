using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
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

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new BlackJack());

            gamePlay.GivePhaseOneCards();

            actor.Hand.Count.Should().Be(3);
        }

        [Fact]
        public void BlackJack_receives_three_cards_if_the_second_is_diamond()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().HeartsAce());
            deck.Put(new StagecoachCardType().DiamondsThree());
            deck.Put(new MissedCardType().ClubsSeven());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new BlackJack());

            gamePlay.GivePhaseOneCards();

            actor.Hand.Count.Should().Be(3);
        }

        [Fact]
        public void BlackJack_receives_two_cards_if_the_second_is_neither_herts_nor_diamond()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().HeartsAce());
            deck.Put(new StagecoachCardType().ClubsSeven());
            deck.Put(new MissedCardType().DiamondsThree ());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new BlackJack());

            gamePlay.GivePhaseOneCards();

            actor.Hand.Count.Should().Be(2);
        }

        [Fact]
        public void Not_BlackJack_receives_two_cards()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new BangCardType().DiamondsThree());
            deck.Put(new StagecoachCardType().HeartsAce());
            deck.Put(new MissedCardType().ClubsSeven());

            var gamePlay = InitGame(deck);
            var actor = SetCharacter(gamePlay, new SlabTheKiller());

            gamePlay.GivePhaseOneCards();

            actor.Hand.Count.Should().Be(2);
        }

        #endregion

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

        private Player SetCharacter(Game.Gameplay gameplay, Character character)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, character);

            return actor;
        }
    }
}
