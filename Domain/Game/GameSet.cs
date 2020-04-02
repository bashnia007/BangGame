using Domain.PlayingCards;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public class GameSet
    {
        public Queue<PlayingCard> Deck { get; }
        public Queue<Role.Role> Roles { get; }
        public Queue<Character.Character> Characters { get; }

        public GameSet(Queue<IShuffledCard> playingCards, Queue<IShuffledCard> roles, Queue<IShuffledCard> characters)
        {
            Deck = new Queue<PlayingCard>(playingCards.Cast<PlayingCard>().ToList());
            Roles = new Queue<Role.Role>(roles.Cast<Role.Role>().ToList());
            Characters = new Queue<Character.Character>(characters.Cast<Character.Character>().ToList());
        }
    }
}
