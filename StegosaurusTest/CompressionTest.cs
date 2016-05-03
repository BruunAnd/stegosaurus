using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Utility;
using System.Linq;
using System.IO;

namespace StegosaurusTest
{
    [TestClass]
    public class CompressionTest
    {
        [TestMethod]
        public void CompressDecompress_SameOutput()
        {
            byte[] buffer = TestUtility.GetRandomBytes(32 * 1024);

            byte[] compressed = Compression.Compress(buffer);
            byte[] decompress = Compression.Decompress(compressed);

            Assert.IsTrue(decompress.SequenceEqual(buffer));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CompressDecompress_ThrowsInvalidDataException()
        {
            Compression.Decompress(TestUtility.GetRandomBytes(32 * 1024));
        }
    }
}
