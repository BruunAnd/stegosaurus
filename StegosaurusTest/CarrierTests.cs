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
        public void ImageCarrier_SaveLoad_ExpectedOutput()
        {
            for (int i = 0; i < 5; i++)
            {
                Random rand = new Random();
                ImageCarrier carrierMedia = new ImageCarrier();
                carrierMedia.ImageData = new Bitmap(rand.Next(5, 2000), rand.Next(5, 2000));
                byte[] tempArray = new byte[carrierMedia.ImageData.Width * carrierMedia.ImageData.Height * carrierMedia.BytesPerSample];
                Console.WriteLine("Trying a {0}x{1} image..", carrierMedia.ImageData.Width, carrierMedia.ImageData.Height);
                rand.NextBytes(tempArray);

                carrierMedia.ByteArray = tempArray;
                carrierMedia.SaveToFile("out.png");

                ImageCarrier reloadedCarrier = new ImageCarrier();
                reloadedCarrier.LoadFromFile("out.png");
                Assert.IsTrue(tempArray.SequenceEqual(reloadedCarrier.ByteArray));
            }
        }
    }
}
