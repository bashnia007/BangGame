using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
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
            if (!CanPlayBang(gameplay))
                return new ErrorState(state);
            
            state.BangAlreadyPlayed = true;
            
            int missedRequired = gameplay.PlayerTurn.Character is SlabTheKiller? 2 : 1;
            
            BarrelApplyer.ApplyBarrel(gameplay, victim, ref missedRequired);
            
            if (missedRequired == 0) return new DoneState(state);
            
            // Barrels don't help.
            var response = new DefenceAgainstBang {Player = victim, CardsRequired = (byte) missedRequired};
            
            var defenceStrategy = new DefenceAgainstBangStrategy(gameplay.PlayerTurn, missedRequired);
            
            return new WaitingMissedCardAfterBangState(victim, defenceStrategy, state){SideEffect = response};
        }

        private bool CanPlayBang(Gameplay gameplay)
        {
            var hitter = gameplay.PlayerTurn;

            return !state.BangAlreadyPlayed || hitter.PlayerTablet.Weapon.MultipleBang; 
        }
    }
}