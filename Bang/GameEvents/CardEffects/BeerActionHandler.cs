using System.Linq;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class BeerActionHandler : CardActionHandler
    {
        public BeerActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player player, BangGameCard card)
        {
            if (player.LifePoints == player.PlayerTablet.MaximumHealth)
            {
                Logger.Info($"Player {player.Name} already has maximum life point!");
                return new ErrorState(state);
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
            
            
            return new DoneState(state);
        }
    }
}