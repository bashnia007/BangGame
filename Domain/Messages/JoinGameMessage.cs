namespace Domain.Messages
{
    public class JoinGameMessage : Message
    {
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessJoinGameMessage(this);
        }
    }
}
