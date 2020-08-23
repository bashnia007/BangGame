namespace Bang.Exceptions
{
    public class GameAlreadyOverException : BangException
    {
        public GameAlreadyOverException() : base("Game already over")
        {
        }
    }
}