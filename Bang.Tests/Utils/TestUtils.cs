using System;
using System.Diagnostics;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.Tests
{
    public static class TestUtils
    {
        public static BangGameCard DiamondsTwo(this CardType cardType) => new BangGameCard(cardType, Suite.Diamonds, Rank.Two);
        public static BangGameCard DiamondsThree(this CardType cardType) => new BangGameCard(cardType, Suite.Diamonds, Rank.Three);
        public static BangGameCard DiamondsFour(this CardType cardType) => new BangGameCard(cardType, Suite.Diamonds, Rank.Four);
        public static BangGameCard ClubsFive(this CardType cardType) => new BangGameCard(cardType, Suite.Clubs, Rank.Five);
        public static BangGameCard ClubsSix(this CardType cardType) => new BangGameCard(cardType, Suite.Clubs, Rank.Six);
        public static BangGameCard ClubsSeven(this CardType cardType) => new BangGameCard(cardType, Suite.Clubs, Rank.Seven);
        public static BangGameCard SpadesEight(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Eight);
        public static BangGameCard SpadesNine(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Nine);
        public static BangGameCard SpadesTen(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Ten);
        public static BangGameCard HeartsJack(this CardType cardType) => new BangGameCard(cardType, Suite.Hearts, Rank.Jack);
        public static BangGameCard SpadesQueen(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Queen);
        public static BangGameCard HeartsKing(this CardType cardType) => new BangGameCard(cardType, Suite.Hearts, Rank.King);
        public static BangGameCard HeartsAce(this CardType cardType) => new BangGameCard(cardType, Suite.Hearts, Rank.Ace);


        internal static Gameplay InitGameplayWithCharacter(Character character, int playersAmount = 4)
        {
            return new GameplayBuilder(playersAmount)
                .WithCharacter(character)
                .Build();
        }
        
        internal static Gameplay InitGameplayWithoutCharacter(Character character, int playersAmount = 4)
        {
            return new GameplayBuilder(playersAmount)
                .WithoutCharacter(character)
                .Build();
        }
        
        internal static Gameplay InitGameplay(int playersCount = 4)
        {
            return InitGameplay(GamePlayInitializer.BangGameDeck(), GamePlayInitializer.CharactersDeck(), playersCount);
        }
        
        internal static Gameplay InitGameplay(Deck<BangGameCard> deck, int playersAmount = 4)
        {
            return InitGameplay(deck, GamePlayInitializer.CharactersDeck(), playersAmount);
        }
        
        internal static Gameplay InitGameplay(Deck<Character> charactersDeck, int playersAmount = 4)
        {
            return InitGameplay(GamePlayInitializer.BangGameDeck(), charactersDeck, playersAmount);
        }
        
        internal static Gameplay InitGameplay(Deck<BangGameCard> deck, Deck<Character> charactersDeck, int playersAmount = 4)
        {
            return new GameplayBuilder(playersAmount).WithCharacterDeck(charactersDeck).WithDeck(deck).Build();
        }
    }
}