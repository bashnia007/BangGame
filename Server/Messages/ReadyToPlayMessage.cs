using Server.Messages;

namespace Bang.Messages
{
    public class ReadyToPlayMessage : Message
    {
        public bool IsReady { get; set; }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessReadyToPlayMessage(this);
        }
    }
}
