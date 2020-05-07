using System;
using Bang.GameEvents;
using Bang.Messages;

namespace Server.Messages
{
    [Serializable]
    public class BangEventMessage : Message
    {
        public BangGameMessage BangGameEvent { get; set; }
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessGameEventMessage(this);
        }
    }
}