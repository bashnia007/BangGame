using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game
{
    class DefenceAgainstIndiansStrategy : DefenceStrategy
    {
        public DefenceAgainstIndiansStrategy(Player hitter) 
            : base(new BangCardType(), hitter, 1)
        {}
    }
}