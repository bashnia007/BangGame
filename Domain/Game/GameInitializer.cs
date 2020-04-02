using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Game
{
    public static class GameInitializer
    {
        private static List<IShuffledCard> playingCards { get; }
        private static List<IShuffledCard> characters { get; }

        static GameInitializer()
        {
            playingCards = InitializePlayingCards();
            characters = InitializeCharacters();
        }

        public static GameSet CreateGameSet(int playersAmount)
        {
            GameSet gameSet = new GameSet(ShuffleCards(playingCards),
                ShuffleCards(CreateRolesForGame(playersAmount)),
                ShuffleCards(characters));

            return gameSet;
        }

        private static List<IShuffledCard> CreateRolesForGame(int playersAmount)
        {
            List<IShuffledCard> roles = new List<IShuffledCard>
            {
                new Role.Sheriff(),
                new Role.Renegade(),
                new Role.Outlaw(),
                new Role.Outlaw()
            };

            switch (playersAmount)
            {
                case 4:
                    break;
                case 5:
                    roles.Add(new Role.Deputy());
                    break;
                case 6:
                    roles.Add(new Role.Outlaw());
                    roles.Add(new Role.Deputy());
                    break;
                case 7:
                    roles.Add(new Role.Outlaw());
                    roles.Add(new Role.Deputy());
                    roles.Add(new Role.Deputy());
                    break;
                default:
                    throw new ArgumentException("Players amount should be fron 4 to 7. Actual value is " + playersAmount);
            }

            return roles;
        }

        private static Dictionary<Type, int> FillPlayingCardDictionary()
        {
            var playingCards = new Dictionary<Type, int>
            {
                { typeof(VolcanicCard), 2 },
                { typeof(SchofieldCard), 3 },
                { typeof(RemingtonCard), 1 },
                { typeof(CarabineCard), 1 },
                { typeof(WinchesterCard), 1 },
                { typeof(JailCard), 3 },
                { typeof(MustangCard), 2 },
                { typeof(BarrelCard), 2 },
                { typeof(DynamiteCard), 1 },
                { typeof(ScopeCard), 1 },
                { typeof(BangCard), 25 },
                { typeof(MissedCard), 12 },
                { typeof(BeerCard), 6 },
                { typeof(PanicCard), 4 },
                { typeof(CatBalouCard), 4 },
                { typeof(DuelCard), 3 },
                { typeof(GeneralStoreCard), 2 },
                { typeof(StagecoachCard), 2 },
                { typeof(IndiansCard), 2 },
                { typeof(GatlingCard), 1 },
                { typeof(SaloonCard), 1 },
                { typeof(WellsFargoCard), 1 }
            };

            return playingCards;
        }

        private static List<IShuffledCard> InitializePlayingCards()
        {
            List<IShuffledCard> playingCards = new List<IShuffledCard>();

            var playingCardsDictionary = FillPlayingCardDictionary();

            foreach (var cardSet in playingCardsDictionary)
            {
                for (int i = 0; i < cardSet.Value; i++)
                {
                    playingCards.Add((PlayingCard)Activator.CreateInstance(cardSet.Key));
                }
            }

            return playingCards;
        }
        
        private static List<IShuffledCard> InitializeCharacters()
        {
            List<IShuffledCard> characters = new List<IShuffledCard>();

            var charactersTypes = Assembly
                .GetAssembly(typeof(Character.Character))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Character.Character)));


            foreach (var character in charactersTypes)
            {
                var newCharacter = Activator.CreateInstance(character);
                characters.Add(newCharacter as Character.Character);
            }

            return characters;
        }

        public static Queue<IShuffledCard> ShuffleCards(List<IShuffledCard> cardsToShuffle)
        {
            Queue<IShuffledCard> shuffledCards = new Queue<IShuffledCard>();
            var cards = new List<IShuffledCard>(cardsToShuffle);
            int cardsAmount = cards.Count;
            var rnd = new Random();

            while (cardsAmount > 0)
            {
                int number = rnd.Next(cardsAmount);
                var cardByNumber = cards[number];
                shuffledCards.Enqueue(cardByNumber);
                cards.RemoveAt(number);
                cardsAmount--;
            }
            return shuffledCards;
        }
    }
}
