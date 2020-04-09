using System;

namespace Domain.Messages
{
    [Serializable]
    public class LeaveGameMessage : Message
    {
        public bool IsSuccess { get; set; }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessLeaveGameMessage(this);
        }
    }
}
