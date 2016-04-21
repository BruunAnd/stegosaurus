using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Utility;
using System.Diagnostics;

namespace StegosaurusTest
{
    [TestClass]
    public class CompressionTest
    {
        [TestMethod]
        public void Compression_MatchingArrays_ReturnsTrue()
        {

            byte[] actualOutput = Compression.Compress(new byte[] {39, 0, 0, 0, 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 99,
                                                                   100, 96, 96, 96, 7, 226, 220, 196, 18, 189, 130,
                                                                   188, 116, 14, 6, 56, 96, 4, 17, 0, 173, 73, 103,
                                                                   222, 31, 0, 0, 0 });

            byte[] expectedOutput = new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 83, 103, 96, 96, 144, 239, 230,
                                                 96, 0, 1, 22, 134, 228, 148, 132, 132, 4, 246, 71, 119, 142, 8,
                                                 237, 109, 218, 83, 194, 199, 102, 145, 192, 34, 200, 176, 214, 51,
                                                 253, 158, 60, 80, 26, 0, 130, 66, 240, 205, 43, 0, 0, 0 };

            string expectedString = "", actualString = "";
            foreach (byte item in expectedOutput)
            {
                expectedString += item.ToString() + ", ";
            }

            foreach (byte item in actualOutput)
            {
                actualString += item.ToString() + ", ";
            }

            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Decompression_MatchingArrays_ReturnsTrue()
        {
            byte[] inputArray = {39, 0, 0, 0, 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 99,
                                 100, 96, 96, 96, 7, 226, 220, 196, 18, 189, 130,
                                 188, 116, 14, 6, 56, 96, 4, 17, 0, 173, 73, 103,
                                 222, 31, 0, 0, 0 };

            byte[] compressedArray = Compression.Compress(inputArray);

            byte[] decompressedArray = Compression.Decompress(compressedArray);

            string expectedString = "", actualString = "";
            foreach (byte item in inputArray)
            {
                expectedString += item.ToString() + ", ";
            }

            foreach (byte item in decompressedArray)
            {
                actualString += item.ToString() + ", ";
            }

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
