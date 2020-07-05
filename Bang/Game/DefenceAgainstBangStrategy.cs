using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game
{
    class DefenceAgainstBangStrategy : DefenceStrategy
    {
        public DefenceAgainstBangStrategy(Player hitter, int requiredCards) 
            : base(new MissedCardType(), hitter, requiredCards)
        {
        }
    }
}