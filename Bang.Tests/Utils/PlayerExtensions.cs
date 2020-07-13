using System.Linq;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.Roles;

namespace Bang.Tests
{
    public static class PlayerExtensions
    {
        public static void Die(this Player player, Player killer = null)
        {
            player.LoseLifePoint(killer, player.MaximumLifePoints);
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
    }
}