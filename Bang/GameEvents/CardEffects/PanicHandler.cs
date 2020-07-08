using System.Linq;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class PanicHandler : CardActionHandler
    {
        public PanicHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            if (DistanceCalculator.GetDistance(gameplay.Players.ToList(), gameplay.PlayerTurn, victim) != 1)
                return new ErrorState(state);
            
            if (victim.Hand.Count + victim.ActiveCards.Count == 0)
                return new ErrorState(state);

            var response = new ChooseOneCardResponse
            {
                HasHandCards = victim.Hand.Any(),
                Player = gameplay.PlayerTurn,
                ActiveCards = victim.ActiveCards.ToList(),
            };
            
            return new WaitingCardToStealAfterPanicState(gameplay.PlayerTurn, victim, state){SideEffect = response};
        }
    }
}