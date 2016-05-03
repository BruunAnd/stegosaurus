using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus;
using System;
using System.Linq;

namespace StegosaurusTest
{
    [TestClass]
    public class StegoMessageTests
    {
        [TestMethod]
        public void Decode_EncodedTextMessage_CorrectOutput()
        {
            const string testString = "Example string.";

            StegoMessage newMessage = new StegoMessage();
            newMessage.TextMessage = testString;

            // recreate message, skip 4 bytes (length)
            StegoMessage recreatedMessage = new StegoMessage(newMessage.ToByteArray().Skip(4).ToArray());

            Assert.AreEqual(newMessage.TextMessage, recreatedMessage.TextMessage);
        }

        [TestMethod]
        public void Decode_EncodedInputFile_CorrectOutput()
        {
            InputFile testInputFile = new InputFile("test.bin", TestUtility.GetRandomBytes(32 * 1024));

            StegoMessage newMessage = new StegoMessage();
            newMessage.InputFiles.Add(testInputFile);

            // recreate message, skip 4 bytes (length)
            StegoMessage recreatedMessage = new StegoMessage(newMessage.ToByteArray().Skip(4).ToArray());
            InputFile testOutputFile = recreatedMessage.InputFiles[0];

            Assert.AreEqual(testInputFile.Name, testOutputFile.Name);
            Assert.IsTrue(testInputFile.Content.SequenceEqual(testOutputFile.Content));
        }
    }
}
