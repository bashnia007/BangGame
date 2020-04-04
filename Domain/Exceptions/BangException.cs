using System;

namespace Domain.Exceptions
{
    public class BangException : Exception
    {
        public BangException(string message) : base(message)
        {}
    }
}