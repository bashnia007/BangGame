using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bang.Characters;
using Bang.Exceptions;
using Bang.PlayingCards;
using Bang.Roles;
using Bang.Game;
using Gameplay;
using Gameplay.Characters;
using Gameplay.Exceptions;
using Gameplay.Roles;


namespace Bang.Game
{
    public static class GameInitializer
    {
        public static List<BangGameCard> PlayingCards { get; }
        public static List<Character> Characters { get; }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static GameInitializer()
        {
            PlayingCards = InitializePlayingCards();
            Characters = InitializeCharacters();
        }

        public static Deck<BangGameCard> BangGameDeck() => new Deck<BangGameCard>(PlayingCards);

        public static Deck<Character> CharactersDeck() => new Deck<Character>(Characters);

        public static List<Role> CreateRolesForGame(int playersAmount)
        {
            List<Role> roles = new List<Role>
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
                    var card = CardFactory.Create((CardType)Activator.CreateInstance(cardSet.Key));
                    playingCards.Add(card);;
                }
            }

            return playingCards;
        }
        
        private static List<Character> InitializeCharacters()
        {
            List<Character> characters = new List<Character>();

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
