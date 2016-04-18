using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Exceptions
{
    public class StegosaurusException : Exception
    {
        public StegosaurusException(string message)
            : base(message)
        {
        }

        public StegosaurusException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
