namespace Domain.Exceptions
{
    public class DuplicatedCardException : BangException
    {
        public DuplicatedCardException(string card) : base($"You already have {0} in play, you cannot play another one")
        {
        }
    }
}