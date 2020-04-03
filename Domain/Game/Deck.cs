using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public class Deck<T> : Queue<T>
    {
        #region Constructors

        public Deck() : base()
        { }

        public Deck(IEnumerable<T> data) : base(data)
        { }

        #endregion

        public Deck<T> Shuffle()
        {
            var list = this.ToList();
            int cardsAmount = list.Count;
            var rnd = new Random();

            this.Clear();

            while (cardsAmount > 0)
            {
                int number = rnd.Next(cardsAmount);
                var cardByNumber = list[number];
                this.Enqueue(cardByNumber);
                list.RemoveAt(number);
                cardsAmount--;
            }

            return this;
        }
    }
}
