namespace Domain.Messages
{
    public class StartGameMessage : Message
    {
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessStartGameMessage(this);
        }
    }
}
