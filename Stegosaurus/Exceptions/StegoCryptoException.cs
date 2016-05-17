using System;

namespace Stegosaurus.Exceptions
{
    public class StegoCryptoException : StegosaurusException
    {
        public StegoCryptoException(string message)
            : base(message)
        {
        }

        public StegoCryptoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
