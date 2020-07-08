using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System.Collections.Generic;
using System.Linq;
using Bang.Game;

namespace Bang.GameEvents.CardEffects
{
    internal class IndiansActionHandler : CardActionHandler
    {
        public IndiansActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player attackPlayer, BangGameCard card)
        {
            var victimStatesList = new Dictionary<Player, HandlerState>();

            foreach (var victim in gameplay.AlivePlayers.Where(p => p != attackPlayer))
            {
                var response = new DefenceAgainstIndians { Player = victim };

                var state = new WaitingBangCardState(victim, attackPlayer, base.state) { SideEffect = response };
                victimStatesList.Add(victim, state);
            }

            return new WaitingBangCardsAfterIndiansState(victimStatesList, state)
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
