using Bang.Characters;
using Bang.PlayingCards;

namespace Bang.Game
{
    public abstract class DrawChecker
    {
        protected abstract bool ShouldApplyEffect(BangGameCard card);

        public bool Draw(Gameplay gameplay, Character victim)
        {
            var firstCard = gameplay.DealCard();

            bool draw = ShouldApplyEffect(firstCard);
            
            gameplay.Discard(firstCard);

            if (victim is LuckyDuke)
            {
                var secondCard = gameplay.DealCard();

                if (!draw)
                    draw = ShouldApplyEffect(secondCard);
                
                gameplay.Discard(secondCard);
            }

            return draw;
        }
    }
}