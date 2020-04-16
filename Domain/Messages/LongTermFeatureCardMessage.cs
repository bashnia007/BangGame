using Domain.PlayingCards;

namespace Domain.Messages
{
    public class LongTermFeatureCardMessage : Message
    {
        public BangGameCard CardForTablet { get; }
        public bool IsSuccess { get; set; }

        public LongTermFeatureCardMessage(BangGameCard card)
        {
            CardForTablet = card;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessLongTermFeatureCardMessage(this);
        }
    }
}
