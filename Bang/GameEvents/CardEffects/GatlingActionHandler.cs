using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects
{
    internal class GatlingActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player attackPlayer, BangGameCard card)
        {
            var victimStatesList = new Dictionary<Player, HandlerState>();
            var bangHandler = new BangCardHandler();

            foreach (var victim in gameplay.Players.Where(p => p.PlayerTablet.IsAlive && p.Id != attackPlayer.Id))
            {
                victimStatesList.Add(victim, bangHandler.ApplyEffect(gameplay, victim, null));
            }

            return new WaitingMissedCardsAfterGatlingState(victimStatesList, gameplay)
            {
                SideEffect = new MultiplayerDefenceResponse
                {
                    CardTypeRequired = new MissedCardType(),
                    PlayersResponses = victimStatesList.Values.Select(s => s.SideEffect).Cast<Response>().ToList()
                }
            };
        }
    }
}
