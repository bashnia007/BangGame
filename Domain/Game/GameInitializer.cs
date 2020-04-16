using Domain.Characters;
using Domain.Exceptions;
using Domain.PlayingCards;
using Domain.Roles;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Domain.Game
{
    public static class GameInitializer
    {
        public static List<BangGameCard> PlayingCards { get; }
        public static List<IShuffledCard> Characters { get; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                    Logger.Error("Roles were not created. More than 7 players!");
                    throw new AmountPlayersException(playersAmount);
            }

            return roles;
        }

        private static Dictionary<Type, int> FillPlayingCardDictionary()
        {
            var playingCards = new Dictionary<Type, int>
            {
                { typeof(VolcanicCardType), 2 },
                { typeof(SchofieldCardType), 3 },
                { typeof(RemingtonCardType), 1 },
                { typeof(CarabineCardType), 1 },
                { typeof(WinchesterCardType), 1 },
                { typeof(JailCardType), 3 },
                { typeof(MustangCardType), 2 },
                { typeof(BarrelCardType), 2 },
                { typeof(DynamiteCardType), 1 },
                { typeof(ScopeCardType), 1 },
                { typeof(BangCardType), 25 },
                { typeof(MissedCardType), 12 },
                { typeof(BeerCardType), 6 },
                { typeof(PanicCardType), 4 },
                { typeof(CatBalouCardType), 4 },
                { typeof(DuelCardType), 3 },
                { typeof(GeneralStoreCardType), 2 },
                { typeof(StagecoachCardType), 2 },
                { typeof(IndiansCardType), 2 },
                { typeof(GatlingCardType), 1 },
                { typeof(SaloonCardType), 1 },
                { typeof(WellsFargoCardType), 1 }
            };

            return playingCards;
        }

        private static List<BangGameCard> InitializePlayingCards()
        {
            List<BangGameCard> playingCards = new List<BangGameCard>();

            var playingCardsDictionary = FillPlayingCardDictionary();

            foreach (var cardSet in playingCardsDictionary)
            {
                for (int i = 0; i < cardSet.Value; i++)
                {
                    var card = CardFactory.Create(cardSet.Key);
                    playingCards.Add(card);;
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
