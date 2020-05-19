using Server.Messages;

namespace Bang.Messages
{
    public class CreateGameMessage : Message
    {
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessCreateGameMessage(this);
        }
    }
}
