using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game
{
    class DefenceAgainstDuelStrategy : DefenceStrategy
    {
        public DefenceAgainstDuelStrategy(Player hitter) : base(new BangCardType(), hitter, 1)
        {
        }
    }
}