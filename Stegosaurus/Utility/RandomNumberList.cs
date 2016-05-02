using System;
using System.Collections;
using System.Collections.Generic;

namespace Stegosaurus.Utility
{
    public class RandomNumberList : IEnumerable<int>
    {
        // Private variables
        private Random random;
        private HashSet<int> generatedIntegers = new HashSet<int>();
        private int maxValue;

        public RandomNumberList(int _seed, int _maxValue)
        {
            random = new Random(_seed);
            maxValue = _maxValue;
        }

        public IEnumerator<int> GetEnumerator()
        {
            int generatedInt;

            // Check if there are any more integers to generate
            if (generatedIntegers.Count >= maxValue)
                throw new ArgumentOutOfRangeException("No more integers to generate.");

            // Generate an integer which has not yet been generated
            int requestedCount = generatedIntegers.Count + 1;
            do
            {
                generatedInt = random.Next(maxValue);
                generatedIntegers.Add(generatedInt);
            } while (generatedIntegers.Count < requestedCount);

            generatedIntegers.Add(generatedInt);

            yield return generatedInt;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
