namespace Stegosaurus.Exceptions
{
    public class RandomNumbersOutOfRangeException : StegosaurusException
    {
        public RandomNumbersOutOfRangeException()
            : base("No more numbers to generate.")
        {
        }
    }
}
