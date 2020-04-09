using System;

namespace Domain.Players
{
    [Serializable]
    /// <summary>
    /// Real online player
    /// </summary>
    public class PlayerOnline : Player
    {
        public PlayerOnline(string id) : base()
        {
            Id = id;
        }
    }
}
