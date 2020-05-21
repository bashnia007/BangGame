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
            var player = replyActionMessage.Player;
            if (replyActionMessage.Response is DefenceAgainstBang d)
            {
                player.Defense(d.FirstCard, d.SecondCard);
            }
            
            return new Done();
        }
    }
}