using Bang.Game;
using Bang.GameEvents.CardEffects.States;

namespace Bang.GameEvents.CardEffects
{
    internal class StageCoachCardHandler : ReplenishCardHandler
    {
        public StageCoachCardHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        protected override short CardsToTakeAmount => 2;
    }
}