using System;

namespace Stegosaurus.Exceptions
{
    public class RandomNumbersOutOfRangeException : Exception
    {
        public RandomNumbersOutOfRangeException()
            : base("No more numbers to generate.")
        {
        }
    }
}
