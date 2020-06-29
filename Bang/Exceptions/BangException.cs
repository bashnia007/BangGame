using System;

namespace Bang.Exceptions
{
    public class BangException : Exception
    {
        public BangException(string message) : base(message)
        {}
    }
}