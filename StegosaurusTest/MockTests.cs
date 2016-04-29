using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Stegosaurus.Algorithm;
using Stegosaurus;
using Stegosaurus.Carrier;
using System.Drawing;
using Stegosaurus.Cryptography;
using System.Linq;

namespace StegosaurusTest
{
    [TestClass]
    public class MockTests
    {
        [TestMethod]

        public void TestMainImplementation()
        {
            const string coverFile = "cover.png";
            const string testMessageString = "Example text message.";
            const string testKey = "Example Key";
            const string testFileName = "Example.bin";
            byte[] testFileBuffer = new byte[1024 * 64];
            new Random().NextBytes(testFileBuffer);

            ICryptoProvider cryptoProvider = new AESProvider();
            cryptoProvider.CryptoKey = testKey;

            // Test requires a cover file
            if (!File.Exists(coverFile))
            {
                new Bitmap(500, 500).Save(coverFile);
            }

            // Instantiate algorithm
            IStegoAlgorithm algorithm = (IStegoAlgorithm) Activator.CreateInstance(typeof(LSBAlgorithm));
            algorithm.CarrierMedia = new ImageCarrier(coverFile);
            algorithm.CryptoProvider = cryptoProvider;

            // Instantiate StegoMessage
            StegoMessage inMessage = new StegoMessage();
            inMessage.TextMessage = testMessageString;
            inMessage.InputFiles.Add(new InputFile(testFileName, testFileBuffer));
            algorithm.Embed(inMessage);

            // Save to an output file
            algorithm.CarrierMedia.SaveToFile("output.png");

            // Load the output file we just saved
            algorithm.CarrierMedia = new ImageCarrier("output.png");

            // Get outMessage and verify message
            StegoMessage outMessage = algorithm.Extract();

            Assert.AreEqual(testMessageString, outMessage.TextMessage);

            InputFile outputFile = outMessage.InputFiles[0];
            Assert.AreEqual(outputFile.Name, testFileName);
            Assert.AreEqual(outputFile.Content.SequenceEqual(testFileBuffer), true);
        }
    }
}
