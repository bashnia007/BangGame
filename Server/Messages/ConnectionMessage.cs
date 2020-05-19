using System;
using Server.Messages;

namespace Bang.Messages
{
    [Serializable]
    public class ConnectionMessage : Message
    {
        public string Name { get; set; }
    }
}
