﻿namespace Domain.Messages
{
    public class JoinGameMessage : Message
    {
        public bool IsJoined { get; set; }
        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessJoinGameMessage(this);
        }
    }
}
