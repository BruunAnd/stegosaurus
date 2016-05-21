using System;

namespace Stegosaurus.Exceptions
{
    public class StegoMessageException : StegosaurusException
    {
        public StegoMessageException(string message)
            : base(message)
        {
        }

        public StegoMessageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
