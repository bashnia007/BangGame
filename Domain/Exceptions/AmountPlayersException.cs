namespace Domain.Exceptions
{
    public class AmountPlayersException : BangException
    {
        public AmountPlayersException(int playersAmount) : base(
            string.Format(CardName.AmountPlayersExceptionMessageFormat, playersAmount))
        {
        }
    }
}