namespace Server.Messages
{
    public abstract class BangResponseMessage : Message
    {
        public bool Success { get; set; }
        public bool Error { get; set; }

        public bool RequireReply => !Success && !Error;
    }
}