using System.Linq;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class BeerActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player player, BangGameCard card)
        {
            if (player.LifePoints == player.PlayerTablet.MaximumHealth)
            {
                Logger.Info($"Player {player.Name} already has maximum life point!");
                return new ErrorState();
            }

            if (gameplay.Players.Count(p => p.PlayerTablet.IsAlive) > 2)
            {
                player.RegainLifePoint();
                Logger.Info($"Player {player.Name} regains one life point");
            }
            else
            {
                Logger.Info("Beer has no effect if there are only 2 players left in the game");
            }
            
            
            return new DoneState();
        }
    }
}