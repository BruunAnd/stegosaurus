using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus
{
    public class RandomNumberList
    {
        private int[] innerArray;

        private int position = 0, maxValue;

        private Random rand;

        private HashSet<int> innerHashSet = new HashSet<int>();

        public int Next => innerArray[position++];

        public int RemainingElements => innerArray.Length - position;

        public RandomNumberList(int _seed, int _maxValue)
        {
            maxValue = _maxValue;
            rand = new Random(_seed);
        }

        public void AddElements(int _count)
        {
            int newCount = innerHashSet.Count + _count;

            // Generate numbers until we have the desired amount
            // HashSet ensures that we don't add duplicates
            while (innerHashSet.Count < newCount)
                innerHashSet.Add(rand.Next(maxValue));

            // Convert to an array, since accessing HashSets by index is slow
            innerArray = innerHashSet.ToArray();
        }

    }
}
