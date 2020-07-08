using System.Linq;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class CatBalouHandler : CardActionHandler
    {
        public CatBalouHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            if (victim.Hand.Count + victim.ActiveCards.Count == 0)
                return new ErrorState(state);

            var response = new ChooseOneCardResponse
            {
                HasHandCards = victim.Hand.Any(),
                Player = gameplay.PlayerTurn,
                ActiveCards = victim.ActiveCards.ToList(),
            };
            
            return new WaitingCardToDropAfterCatBalouState(victim, state){SideEffect = response}; 
        }
    }
}