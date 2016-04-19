using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Stegosaurus.Algorithm;
using System.Text;
using Stegosaurus;
using Stegosaurus.Carrier;
using System.Drawing;

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

            // Test requires a cover file
            if (!File.Exists(coverFile))
            {
                new Bitmap(200, 200).Save(coverFile);
            }

            // Instantiate algorithm
            IStegoAlgorithm algorithm = (IStegoAlgorithm) Activator.CreateInstance(typeof(LSBAlgorithm));
            algorithm.CarrierMedia = new ImageCarrier(coverFile);
            algorithm.Key = Encoding.UTF8.GetBytes(testKey);

            // Instantiate StegoMessage
            StegoMessage inMessage = new StegoMessage();
            inMessage.TextMessage = testMessageString;
            algorithm.Embed(inMessage);

            // Save to an output file
            algorithm.CarrierMedia.SaveToFile("output.png");

            // Load the output file we just saved
            algorithm.CarrierMedia = new ImageCarrier("output.png");

            // Get outMessage and verify message
            StegoMessage outMessage = algorithm.Extract();

            Assert.AreEqual(testMessageString, outMessage.TextMessage);
        }
    }
}
