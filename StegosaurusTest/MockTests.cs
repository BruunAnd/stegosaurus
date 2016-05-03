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
        public void MainImplementation_DefaultAlgorithms_ExpectedOutput()
        {
            const string testMessageString = "Example text message.";
            const string testKey = "Example Key";
            const string testFileName = "Example.bin";
            byte[] testFileBuffer = new byte[1024 * 32];
            new Random().NextBytes(testFileBuffer);

            ICryptoProvider cryptoProvider = new AESProvider();
            cryptoProvider.SetKey(testKey);

            // Test requires a cover file
            Image testImage = new Bitmap(500, 500);

            // Instantiate algorithm
            IStegoAlgorithm algorithm = (IStegoAlgorithm) Activator.CreateInstance(typeof(LSBAlgorithm));
            algorithm.CarrierMedia = new ImageCarrier(testImage);
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
            Assert.IsTrue(outputFile.Content.SequenceEqual(testFileBuffer));
        }
    }
}
