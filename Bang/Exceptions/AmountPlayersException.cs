using Gameplay.Exceptions;

namespace Bang.Exceptions
{
    public class AmountPlayersException : BangException
    {
        public AmountPlayersException(int playersAmount) : base(
            string.Format(CardName.AmountPlayersExceptionMessageFormat, playersAmount))
        {
        }
    }
}