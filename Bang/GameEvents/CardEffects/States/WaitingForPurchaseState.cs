using Bang.Players;
using Bang.PlayingCards;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingForPurchaseState : HandlerState
    {
        private List<BangGameCard> cardsToChoose;
        private List<Player> playersOrder;

        public WaitingForPurchaseState(List<Player> playersOrder, List<BangGameCard> cardsToChoose)
        {
            this.cardsToChoose = cardsToChoose;
            this.playersOrder = playersOrder;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard card)
        {
            Player currentPlayer = playersOrder[0];
            if (player.Id != currentPlayer.Id)
            {
                string errorMsg = $"Wrong player's turn! There is turn of player {currentPlayer.Name}, " +
                    $"but {player.Name} made a choice!";
                Log.Error(errorMsg);
                throw new ArgumentException(errorMsg);
            }
            if (!cardsToChoose.Contains(card))
            {
                string errorMsg = $"Wrong card! There is no card {card.Description} in the store! " +
                    $"Cards in the store are:\n {string.Join(",", cardsToChoose.Select(c => c.Description))}";
                Log.Error(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            player.AddCardToHand(card);
            cardsToChoose.Remove(card);
            playersOrder.Remove(currentPlayer);
            Log.Info($"Player {player.Name} chose card {card.Description}");

            if (playersOrder.Count == 0) return new DoneState();

            SideEffect = new ChooseCardsResponse
            {
                CardsToChoose = cardsToChoose,
                PlayerTurn = playersOrder[0]
            };

            return this;
        }
    }
}
