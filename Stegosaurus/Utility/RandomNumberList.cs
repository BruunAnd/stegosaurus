using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stegosaurus.Utility
{
    public class RandomNumberList : IEnumerable<int>
    {
        // Private variables
        private Random random;
        private List<int> generatedIntegers = new List<int>();
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
            if (generatedIntegers.Count >= maxValue + 1)
                throw new ArgumentOutOfRangeException("No more integers to generate.");

            // Generate an integer which has not yet been generated
            do
            {
                generatedInt = random.Next(maxValue + 1);
            } while (generatedIntegers.Contains(generatedInt));

            generatedIntegers.Add(generatedInt);

            yield return generatedInt;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
