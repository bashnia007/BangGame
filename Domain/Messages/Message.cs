using System;

namespace Domain.Messages
{
    [Serializable]
    public abstract class Message
    {
        public string GameId { get; set; }
    }
}
