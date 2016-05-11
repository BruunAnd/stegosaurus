using System;
using System.Collections;
using System.Collections.Generic;

namespace Stegosaurus.Utility
{
    public class RandomNumberList : IEnumerable<int>
    {
        // Private variables
        private readonly Random random;
        private readonly HashSet<int> generatedIntegers = new HashSet<int>();
        private readonly int maxValue;

        public RandomNumberList(int _seed, int _maxValue)
        {
            random = new Random(_seed);
            maxValue = _maxValue + 1;
        }

        public IEnumerator<int> GetEnumerator()
        {
            int generatedInt;

            // Check if there are any more integers to generate
            if (generatedIntegers.Count >= maxValue + 1)
                yield break;

            // Generate an integer which has not yet been generated
            do
            {
                generatedInt = random.Next(maxValue);
            } while (!generatedIntegers.Add(generatedInt));

            yield return generatedInt;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
