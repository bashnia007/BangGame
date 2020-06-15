using System.Linq;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class SaloonCardHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            foreach (var player in gameplay.Players.Where(p => p.LifePoints > 0))
            {
                player.RegainLifePoint();
            }
            
            return new DoneState();
        }
    }
}