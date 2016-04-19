using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus;
using System.Linq;

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
    }
}
