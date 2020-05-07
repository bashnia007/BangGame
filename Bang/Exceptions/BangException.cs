using System;

namespace Gameplay.Exceptions
{
    public class BangException : Exception
    {
        public BangException(string message) : base(message)
        {}
    }
}