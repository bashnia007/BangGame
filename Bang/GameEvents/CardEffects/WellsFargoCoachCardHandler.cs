using Bang.Game;
using Bang.GameEvents.CardEffects.States;

namespace Bang.GameEvents.CardEffects
{
    internal class WellsFargoCoachCardHandler : ReplenishCardHandler
    {
        public WellsFargoCoachCardHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        protected override short CardsToTakeAmount => 3;
    }
}