namespace Domain.Exceptions
{
    public class DuplicatedCardException : BangException
    {
        public DuplicatedCardException(string card) : base(
            string.Format(CardName.DuplicatedCardExceptionMessageFormat, card))
        {
        }
    }
}