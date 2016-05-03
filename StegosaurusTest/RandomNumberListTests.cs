using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Utility;
using System.Linq;
using System.Collections.Generic;

namespace StegosaurusTest
{
    [TestClass]
    public class RandomNumberListTests
    {
        [TestMethod]
        public void GenerateNumbers_HasNoDuplicates()
        {
            const int count = 1000;
            int duplicateCount = 0;
            IEnumerable<int> randomNumbers = new RandomNumberList(0, count);
            List<int> existingNumbers = new List<int>();

            // check for duplicates
            for (int i = 0; i < count; i++)
            {
                int nextInt = randomNumbers.First();
                if (existingNumbers.Contains(nextInt))
                {
                    duplicateCount++;
                }
                existingNumbers.Add(nextInt);
            }

            Assert.AreEqual(duplicateCount, 0);
        }
    }
}
