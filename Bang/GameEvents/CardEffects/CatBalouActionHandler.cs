using System.Collections.Generic;
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

            var codeToCard = new Dictionary<ClosedHandCard, BangGameCard>();
            for (int i = 0; i < victim.Hand.Count; i++)
            {
                codeToCard[new ClosedHandCard {Code = i}] = victim.Hand[i];
            }
            
            var response = new ChooseOneCardResponse
            {
                HandCardCodes = codeToCard.Keys.ToList(),
                Player = gameplay.PlayerTurn,
                ActiveCards = victim.ActiveCards.ToList(),
            };
            
            return new WaitingCardToDropAfterCatBalouState(victim, gameplay, codeToCard){SideEffect = response}; 
        }
    }
}