using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using NLog.Fluent;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects
{
    internal class GeneralStoreActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player player, BangGameCard card)
        {
            var cardsToChoose = new List<BangGameCard>();
            List<Player> playersOrder = new List<Player>();

            var playersAlive = gameplay.Players.Where(p => p.PlayerTablet.IsAlive).ToList();
            int indexOfCurrentPlayer = playersAlive.IndexOf(player);

            for (int i = indexOfCurrentPlayer; i < indexOfCurrentPlayer + playersAlive.Count; i++)
            {
                playersOrder.Add(playersAlive[i % playersAlive.Count]);
            }

            for (int i = 0; i < playersOrder.Count(); i++)
            {
                cardsToChoose.Add(gameplay.DealCard());
            }

            Log.Debug($"Player {player.Name} played General Store card. Cards in the store:\n" +
                $"{string.Join(", ", cardsToChoose.Select(c => c.Description))}");

            return new WaitingForPurchaseState(playersOrder, cardsToChoose)
            {
                SideEffect = new ChooseCardsResponse
                {
                    CardsToChoose = cardsToChoose,
                    PlayerTurn = player
                }
            };
        }
    }
}
