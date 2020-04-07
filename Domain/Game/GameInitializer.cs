using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Characters;
using Domain.Exceptions;
using Domain.Roles;

namespace Domain.Game
{
    public static class GameInitializer
    {
        public static List<IShuffledCard> PlayingCards { get; }
        public static List<IShuffledCard> Characters { get; }

        static GameInitializer()
        {
            PlayingCards = InitializePlayingCards();
            Characters = InitializeCharacters();
        }
        
        public static List<IShuffledCard> CreateRolesForGame(int playersAmount)
        {
            List<IShuffledCard> roles = new List<IShuffledCard>
            {
                new Sheriff(),
                new Renegade(),
                new Outlaw(),
                new Outlaw()
            };

            switch (playersAmount)
            {
                case 4:
                    break;
                case 5:
                    roles.Add(new Deputy());
                    break;
                case 6:
                    roles.Add(new Outlaw());
                    roles.Add(new Deputy());
                    break;
                case 7:
                    roles.Add(new Outlaw());
                    roles.Add(new Deputy());
                    roles.Add(new Deputy());
                    break;
                default:
                    throw new AmountPlayersException(playersAmount);
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
                .GetAssembly(typeof(Character))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Character)));


            foreach (var character in charactersTypes)
            {
                var newCharacter = Activator.CreateInstance(character);
                characters.Add(newCharacter as Character);
            }

            return characters;
        }
    }
}
