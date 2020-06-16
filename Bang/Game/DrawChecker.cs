using Bang.Characters;
using Bang.PlayingCards;
using NLog;

namespace Bang.Game
{
    public abstract class DrawChecker
    {
        protected abstract bool ShouldApplyEffect(BangGameCard card);

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool Draw(Gameplay gameplay, Character victim)
        {
            Logger.Info("Check top card in the deck");

            var firstCard = gameplay.DealCard();

            bool draw = ShouldApplyEffect(firstCard);
            Logger.Info($"Card is {firstCard}. Check is {draw}");
            
            gameplay.Discard(firstCard);

            if (victim is LuckyDuke)
            {
                Logger.Info("Lucky Duke checks second card");
                var secondCard = gameplay.DealCard();

                if (!draw)
                    draw = ShouldApplyEffect(secondCard);

                Logger.Info($"Card is {secondCard}. Final check is {draw}");

                gameplay.Discard(secondCard);
            }

            return draw;
        }

        public bool Peek(Gameplay gameplay)
        {
            var card = gameplay.GetTopCardFromDeck();
            Logger.Info("Top card in the deck is " + card.ToString());

            return ShouldApplyEffect(card);
        }
    }
}