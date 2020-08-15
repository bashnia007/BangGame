using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class BangCardHandler : CardActionHandler
    {
        public BangCardHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            var canPlayBang = CanPlayBang(gameplay, victim);
            if (!canPlayBang)
            {
                return new ErrorState(state, canPlayBang.Reason);
            }
            
            state.BangAlreadyPlayed = true;
            
            int missedRequired = gameplay.PlayerTurn.Character is SlabTheKiller? 2 : 1;
            
            BarrelApplyer.ApplyBarrel(gameplay, victim, ref missedRequired);
            
            if (missedRequired == 0) return new DoneState(state);
            
            // Barrels don't help.
            var response = new DefenceAgainstBang {Player = victim, CardsRequired = (byte) missedRequired};
            
            var defenceStrategy = new DefenceAgainstBangStrategy(gameplay.PlayerTurn, missedRequired);
            
            return new WaitingMissedCardAfterBangState(victim, defenceStrategy, state){SideEffect = response};
        }

        private Result CanPlayBang(Gameplay gameplay, Player target)
        {
            var hitter = gameplay.PlayerTurn;

            if (state.BangAlreadyPlayed && !hitter.PlayerTablet.Weapon.MultipleBang)
            {
                // TODO use resource
                return Result.Error("You already played bang card in this turn!");
            }

            var distance = DistanceCalculator.GetDistance(gameplay.AlivePlayers.ToList(), hitter, target);

            if (distance > hitter.Weapon.Distance)
            {
                // TODO use resource
                return Result.Error(
                    $"Distance between you and {target.Name} is {distance}. " +
                    $"Your maximum reachable shooting distance is {hitter.Weapon.Distance}");
            }
            
            return Result.Success(); 
        }
    }
}