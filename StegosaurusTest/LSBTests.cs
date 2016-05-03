using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Utility;
using System.Linq;

namespace StegosaurusTest
{
    [TestClass]
    public class RandomNumberListTest
    {
        [TestMethod]
        public void GenerateNumbers_ThrowsOutOfRangeException()
        {
            const int count = 1000;
            new RandomNumberList(0, count).Take(count + 1);
        }

        [TestMethod]
        public void GenerateNumbers_NoDuplicates()
        {
            const int count = 1000;
            int duplicateCount = 0;
            int[] randomInts = new RandomNumberList(0, count).Take(1000).ToArray();

            // check for duplicates
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else if (randomInts[i] == randomInts[j])
                    {
                        duplicateCount++;
                    }
                }
            }

            Assert.Equals(duplicateCount, 0);
        }
    }
}
