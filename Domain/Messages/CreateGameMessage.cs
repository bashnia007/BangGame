namespace Domain.Messages
{
    public class CreateGameMessage : Message
    {
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessCreateGameMessage(this);
        }
    }
}
