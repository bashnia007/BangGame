using System;

namespace Bang.Players
{
    /// <summary>
    /// Real online player
    /// </summary>
    [Serializable]
    public class PlayerOnline : Player
    {
        public PlayerOnline(string id) : base()
        {
            Id = id;
        }
    }
}
