using System.Linq;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class CatBalouHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            if (victim.Hand.Count + victim.ActiveCards.Count == 0)
                return new ErrorState();

            var response = new ChooseOneCardResponse
            {
                HasHandCards = victim.Hand.Any(),
                Player = gameplay.PlayerTurn,
                ActiveCards = victim.ActiveCards.ToList(),
            };
            
            return new WaitingCardToDropAfterCatBalouState(victim, gameplay){SideEffect = response}; 
        }
    }
}