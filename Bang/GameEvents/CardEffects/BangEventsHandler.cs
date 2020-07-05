using System;

namespace Bang.GameEvents
{
    public class BangEventsHandler
    {
        private readonly Game.Gameplay gamePlay;
        public BangEventsHandler(Game.Gameplay gameplay)
        {
            this.gamePlay = gameplay ?? throw new ArgumentNullException(nameof(gameplay));
        }
        
        public Response DealCard(TakeCardsEvent gameEvent)
        {
            var player = gameEvent.Player;
            for (int i = 0; i < gameEvent.CardsToTakeAmount; i++)
            {
                var card = gamePlay.DealCard();
                player.AddCardToHand(card);
            }

            return new Done();
        }

        public Response DropCard(DropCardsMessage message)
        {
            var player = message.Player;
            player.DropCards(message.CardsToDrop);

            // TODO possible it makes sense to add some OK message to inner events
            return new Done();
        }

        public Response PlayCard(PlayCardMessage message)
        {
            var player = message.Player;
            
            return player.PlayCard(message.Card, message.PlayAgainst);
        }

        public Response CheckDrawCard(CheckDrawCardMessage checkDrawCardMessage)
        {
            throw new NotImplementedException();
        }

        public Response ReplyAction(ReplyActionMessage replyActionMessage)
        {
            Response response = new Done();
            
            response = replyActionMessage.Response switch
            {
                // Bang or gatling reply
                DefenceAgainstBang bangReply => gamePlay.ProcessReplyAction(bangReply.Player, bangReply.FirstCard,
                    bangReply.SecondCard),
                
                // Indians reply
                DefenceAgainstIndians indiansReply => gamePlay.ProcessReplyAction(indiansReply.Player, indiansReply.Card),
                
                // duel reply
                DefenceAgainstDuel duelReply => gamePlay.ProcessReplyAction(duelReply.Player, duelReply.Card),
                
                // Cat Balou
                ForcePlayerToDropCardResponse forceToDrop when forceToDrop.RandomHandCard => gamePlay.ProcessReplyAction(forceToDrop.Player),
                ForcePlayerToDropCardResponse forceToDrop =>gamePlay.ProcessReplyAction(forceToDrop.Player, forceToDrop.ActiveCardToDrop),
                
                // Panic
                DrawCardFromPlayerResponse stealCard when stealCard.RandomHandCard => gamePlay.ProcessReplyAction(stealCard.Player),
                DrawCardFromPlayerResponse stealCard => gamePlay.ProcessReplyAction(stealCard.Player, stealCard.ActiveCardToSteal),
                
                // General store reply
                TakeCardAfterGeneralStoreResponse chooseCard => gamePlay.ProcessReplyAction(chooseCard.Player, chooseCard.Card),
                
                _ => throw new NotImplementedException(replyActionMessage.Response.ToString()) 
            };
            
            return response;
        }
    }
}