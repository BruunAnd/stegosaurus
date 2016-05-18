using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Carrier;

namespace StegosaurusTest
{
    [TestClass]
    public class CarrierTests
    {
        [TestMethod]
        public void ImageCarrier_SaveLoad_ExpectedOut()
        {
            Random rand = new Random();
            ImageCarrier carrierMedia = new ImageCarrier(new Bitmap(rand.Next(500, 1000), rand.Next(500, 1000)));
            byte[] tempArray = new byte[carrierMedia.InnerImage.Width * carrierMedia.InnerImage.Height * carrierMedia.BytesPerSample];
            rand.NextBytes(tempArray);

            carrierMedia.ByteArray = tempArray;
            carrierMedia.SaveToFile("out.png");

            ImageCarrier reloadedCarrier = new ImageCarrier("out.png");
            Assert.IsTrue(tempArray.SequenceEqual(reloadedCarrier.ByteArray));
        }
    }
}
