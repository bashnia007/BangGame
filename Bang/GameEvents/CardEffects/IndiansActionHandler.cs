using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bang.GameEvents.CardEffects
{
    internal class IndiansActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player attackPlayer, BangGameCard card)
        {
            var victimStatesList = new Dictionary<Player, HandlerState>();

            foreach (var victim in gameplay.Players.Where(p => p.PlayerTablet.IsAlive & p.Id != attackPlayer.Id))
            {
                var response = new DefenceAgainstIndians { Player = victim };

                var state = new WaitingBangCardState(victim) { SideEffect = response };
                victimStatesList.Add(victim, state);
            }

            return new WaitingBangCardsAfterIndiansState(victimStatesList, gameplay)
            {
                SideEffect = new MultiplayerDefenceResponse
                {
                    CardTypeRequired = new BangCardType(),
                    PlayersResponses = victimStatesList.Values.Select(s => s.SideEffect).Cast<Response>().ToList()
                }
            };
        }
    }
}
