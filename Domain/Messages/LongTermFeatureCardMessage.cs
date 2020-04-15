using Domain.PlayingCards;

namespace Domain.Messages
{
    public class LongTermFeatureCardMessage : Message
    {
        public LongTermFeatureCard LongTermFeatureCard { get; }
        public bool IsSuccess { get; set; }

        public LongTermFeatureCardMessage(LongTermFeatureCard card)
        {
            LongTermFeatureCard = card;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessLongTermFeatureCardMessage(this);
        }
    }
}
