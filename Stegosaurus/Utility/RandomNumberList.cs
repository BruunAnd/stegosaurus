using Stegosaurus.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Stegosaurus.Utility
{
    public class RandomNumberList
    {
        private readonly Random random;
        private readonly HashSet<int> generatedIntegers = new HashSet<int>();
        private readonly int maxValue;

        /// <summary>
        /// Construct a RandomNumberList with a seed and maximum value.
        /// </summary>
        public RandomNumberList(int _seed, int _maxValue)
        {
            random = new Random(_seed);
            maxValue = _maxValue;
        }

        /// <summary>
        /// Get the next integer in the random number sequence.
        /// Throws a RandomNumbersOutOfRangeException if there are no numbers left to generate.
        /// </summary>
        public int Next
        {
            get
            {
                int generatedInt;

                // Check if there are any more integers to generate.
                if (generatedIntegers.Count >= maxValue)
                {
                    throw new RandomNumbersOutOfRangeException();
                }

                // Generate an integer which has not yet been generated.
                do
                {
                    generatedInt = random.Next(maxValue);
                } while (!generatedIntegers.Add(generatedInt));

                return generatedInt;
            }
        }
    }
}
