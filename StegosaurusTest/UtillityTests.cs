using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Utility;


namespace StegosaurusTest
{
    [TestClass]
    public class UtillityTests
    {
        [TestMethod]
        public void Compression_DecompressCompressedData_CorrectOutput ()
        {
            byte[] buffer = TestUtility.GetRandomBytes(32 * 1024);

            byte[] compressed = Compression.Compress(buffer);
            byte[] decompress = Compression.Decompress(compressed);

            Assert.IsTrue(decompress.SequenceEqual(buffer));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Compression_DecompressRandomData_ThrowsInvalidDataException ()
        {
            Compression.Decompress(TestUtility.GetRandomBytes(32 * 1024));
        }

        [TestMethod]
        public void RandomNumberList_GenerateNumbers_HasNoDuplicates ()
        {
            const int count = 1000;
            int duplicateCount = 0;
            RandomNumberList randomNumbers = new RandomNumberList(0, count);
            List<int> existingNumbers = new List<int>();

            // check for duplicates
            for (int i = 0; i < count; i++)
            {
                int nextInt = randomNumbers.Next;
                if (existingNumbers.Contains(nextInt))
                {
                    duplicateCount++;
                }
                existingNumbers.Add(nextInt);
            }

            Assert.AreEqual(duplicateCount, 0);
        }

        [TestMethod]
        public void SizeFormatter_StringFormatBytes_ExpectedFormat ()
        {
            string expectedOutput = "1 KiB";
            long bytes = 1024;

            string actualOutput = SizeFormatter.StringFormatBytes(bytes);
            
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
