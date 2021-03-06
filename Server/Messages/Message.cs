﻿using System;
using Bang.Messages;

namespace Server.Messages
{
    [Serializable]
    public abstract class Message
    {
        public string GameId { get; set; }
        public string PlayerId { get; set; }

        public virtual void Accept(IMessageProcessor visitor)
        { }
    }
}
