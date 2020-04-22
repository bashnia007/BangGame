using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    [Serializable]
    public class Deck<T>
    {
        private Stack<T> cards;
        #region Constructors

        
        public Deck(IEnumerable<T> data)
        {
            cards = new Stack<T>(data);
        }
        
        public Deck() : this(new List<T>()){}

        #endregion

        public Deck<T> Shuffle()
        {
            var list = cards.ToList();
            int cardsAmount = list.Count;
            var rnd = new Random();

            cards = new Stack<T>();

            while (cardsAmount > 0)
            {
                int number = rnd.Next(cardsAmount);
                var cardByNumber = list[number];
                cards.Push(cardByNumber);
                list.RemoveAt(number);
                cardsAmount--;
            }

            return this;
        }

        public bool IsEmpty() => cards.Count == 0;

        public T Deal()
        {
            if (IsEmpty()) throw new InvalidOperationException();

            return cards.Pop();
        }

        public void Put(T card) => cards.Push(card);
    }
}
