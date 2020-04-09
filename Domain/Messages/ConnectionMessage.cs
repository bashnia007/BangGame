using System;

namespace Domain.Messages
{
    [Serializable]
    public class ConnectionMessage : Message
    {
        public string Name { get; set; }
    }
}
