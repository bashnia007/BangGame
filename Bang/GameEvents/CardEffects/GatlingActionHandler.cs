using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System.Collections.Generic;
using System.Linq;
using Bang.Game;

namespace Bang.GameEvents.CardEffects
{
    internal class GatlingActionHandler : CardActionHandler
    {
        public GatlingActionHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player attackPlayer, BangGameCard card)
        {
            var victims = new List<Player>();

            foreach (var victim in gameplay.AlivePlayers.Where(p => p != attackPlayer))
            {
                int missedRequired = 1;
                BarrelApplyer.ApplyBarrel(gameplay, victim, ref missedRequired);

                if (missedRequired > 0)
                {
                    victims.Add(victim);
                }
            }

            if (victims.Any())
            {
                var responses = new List<Response>();
                foreach (var victim in victims)
                {
                    responses.Add(new DefenceAgainstBang{Player = victim, CardsRequired = 1});
                }

                return new WaitingMissedCardsAfterGatlingState(victims, state)
                {
                    SideEffect = new MultiplayerDefenceResponse
                    {
                        CardTypeRequired = new MissedCardType(),
                        PlayersResponses = responses
                    }
                }; 
            }

            return new DoneState(state);
        }
    }
}
