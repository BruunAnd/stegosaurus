using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Algorithm;
using Stegosaurus;
using Stegosaurus.Carrier;
using System.Drawing;
using Stegosaurus.Cryptography;
using System.Linq;
using System.Threading;

namespace StegosaurusTest
{
    [TestClass]
    public class MockTests
    {
        public void TestSpecifiedAlgorithms(ICryptoProvider _cryptoProvider, StegoAlgorithmBase _algorithm, int _dataSize)
        {
            const string testMessageString = "Example text message.";
            const string testFileName = "Example.bin";
            byte[] testFileBuffer = new byte[1024 * _dataSize];
            new Random().NextBytes(testFileBuffer);

            // Setup cryptoProvider
            RSAKeyPair rsaPair = null;
            if (_cryptoProvider is RSAProvider)
            {
                rsaPair = RSAProvider.GenerateKeys(_cryptoProvider.KeySize);
                _cryptoProvider.SetKey(rsaPair.PublicKey);
            }
            else
            {
                _cryptoProvider.Key = _cryptoProvider.GenerateKey();
            }

            // Test requires a cover file
            Bitmap testImage = new Bitmap(500, 500);

            // Setup algorithm
            _algorithm.CarrierMedia = new ImageCarrier(testImage);
            new Random().NextBytes(_algorithm.CarrierMedia.ByteArray);
            _algorithm.CryptoProvider = _cryptoProvider;

            // Instantiate StegoMessage
            StegoMessage inMessage = new StegoMessage {TextMessage = testMessageString};
            inMessage.InputFiles.Add(new InputFile(testFileName, testFileBuffer));
            _algorithm.Embed(inMessage, null, CancellationToken.None);

            // Save to an output file
            _algorithm.CarrierMedia.SaveToFile("output.png");

            // Load the output file we just saved
            _algorithm.CarrierMedia = new ImageCarrier("output.png");

            // Change key if using RSA
            if (_cryptoProvider is RSAProvider)
            {
                _cryptoProvider.SetKey(rsaPair.PrivateKey);
            }

            // Get outMessage and verify message
            StegoMessage outMessage = _algorithm.Extract();

            Assert.AreEqual(testMessageString, outMessage.TextMessage);

            InputFile outputFile = outMessage.InputFiles[0];
            Assert.AreEqual(outputFile.Name, testFileName);
            Assert.IsTrue(outputFile.Content.SequenceEqual(testFileBuffer));
        }

        [TestMethod]
        public void GTA_AES_ExpectedOutput()
        {
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                TestSpecifiedAlgorithms(new AESProvider(), new GraphTheoreticAlgorithm(), rand.Next(4, 24));
            }
        }

        [TestMethod]
        public void GTA_RSA_ExpectedOutput()
        {
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                TestSpecifiedAlgorithms(new RSAProvider(), new GraphTheoreticAlgorithm(), rand.Next(4, 24));
            }
        }

        [TestMethod]
        public void LSB_AES_ExpectedOutput()
        {
            TestSpecifiedAlgorithms(new AESProvider(), new LSBAlgorithm(), 16);
        }

        [TestMethod]
        public void LSB_RSA_ExpectedOutput()
        {
            TestSpecifiedAlgorithms(new RSAProvider(), new LSBAlgorithm(), 16);
        }

        [TestMethod]
        public void CSA_AES_ExpectedOutput()
        {
            TestSpecifiedAlgorithms(new AESProvider(), new CommonSampleAlgorithm(), 3);
        }

        [TestMethod]
        public void CSA_RSA_ExpectedOutput()
        {
            TestSpecifiedAlgorithms(new RSAProvider(), new CommonSampleAlgorithm(), 3);
        }
    }
}
