using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus;
using System.Linq;
using System.Diagnostics;

namespace StegosaurusTest
{
    [TestClass]
    public class StegoMessageTests
    {
        [TestMethod]
        public void TestEncodeDecode()
        {
            const string testString = "Example string.";

            // Instantiate Message
            StegoMessage newMessage = new StegoMessage();
            newMessage.TextMessage = testString;

            // Skip 4 bytes because the constructor does not handle the length
            byte[] asByteArray = newMessage.ToByteArray().Skip(4).ToArray();

            // Create Message from the ByteArray of newMessage
            StegoMessage recreatedMessage = new StegoMessage(asByteArray);

            Assert.AreEqual(newMessage.TextMessage, recreatedMessage.TextMessage);
        }
        [TestMethod]
        public void ToByteArray_MatchingByteArrays_ReturnsTrue()
        {
            StegoMessage stegoMessage = new StegoMessage();

            stegoMessage.InputFiles.Add(new InputFile("mat.png", new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));

            byte[] expectedOutput = new byte[] { 32, 0, 0, 0, 1, 1, 0, 0, 0, 7, 0, 0, 0, 109, 97, 116, 46, 112, 110,
                                                 103, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };


            string expectedString = "", actualString="";
            foreach (byte item in expectedOutput)
            {
                expectedString += item.ToString() + ", ";
            }

            foreach (byte item in stegoMessage.ToByteArray())
            {
                actualString += item.ToString() + ", ";
            }
            
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
