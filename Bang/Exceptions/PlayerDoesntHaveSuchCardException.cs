using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Exceptions
{
    public class PlayerDoesntHaveSuchCardException : BangException
    {
        public PlayerDoesntHaveSuchCardException(Player player, BangGameCard card) : 
            base($"Player {player.Name} doesn't have {card.Description}")
        {}
    }
}