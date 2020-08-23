using System;
using System.Linq;
using Bang.Exceptions;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.Tests
{
    public static class PlayerExtensions
    {
        public static void WithOneLifePoint(this Player player)
        {
            player.LoseLifePoint(player.LifePoints - 1);
        }
        
        public static void Die(this Player player, Player becauseOf = null)
        {
            player.LoseLifePoint(becauseOf, player.MaximumLifePoints);
        }
        
        public static Player AsSheriff(this Player player, Gameplay gameplay)
        {
            var previousSheriff = gameplay.Players.First(p => p.Role is Sheriff);
            previousSheriff.SetInfo(gameplay, player.Role, previousSheriff.Character);

            player.SetInfo(gameplay, new Sheriff(), player.Character);
            return player;
        }
        
        public static Player AsDeputy(this Player player, Gameplay gameplay)
        {
            player.SetInfo(gameplay, new Deputy(), player.Character);
            return player;
        }
        
        public static Player AsRenegade(this Player player, Gameplay gameplay)
        {
            var previousRenegade = gameplay.Players.First(p => p.Role is Renegade);
            previousRenegade.SetInfo(gameplay, player.Role, previousRenegade.Character);
            
            
            player.SetInfo(gameplay, new Renegade(), player.Character);
            return player;
        }
        
        public static Player AsOutlaw(this Player player, Gameplay gameplay)
        {
            player.SetInfo(gameplay, new Outlaw(), player.Character);
            return player;
        }

        public static Response PlayDuel(this Player player, Player opponent)
        {
            var duelCard = player.Hand.FirstOrDefault(c => c == new DuelCardType());
            if (duelCard == null)
                throw new ArgumentException("Player doesn't have a duel card");

            return player.PlayCard(duelCard, opponent);
        }
        
        public static Response PlayIndians(this Player player) => player.PlayCard(new IndiansCardType());
        
        public static Response PlayGatling(this Player player) => player.PlayCard(new GatlingCardType());

        public static Response PlayDynamite(this Player player) => player.PlayCard(new DynamiteCardType());

        private static Response PlayCard(this Player player, CardType cardType)
        {
            var card = player.Hand.FirstOrDefault(c => c == cardType);
            if (card == null)
                throw new ArgumentException($"Player {player.Name} doesn't have a {cardType}");

            return player.PlayCard(card);
        }
    }
}