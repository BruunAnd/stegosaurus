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
    public class ImageCarrierTests
    {
        [TestMethod]
        public void TestImageCarrier()
        {
            for (int i = 0; i < 500; i++)
            {
                var car = new ImageCarrier("out.png");
                var bytearr = new byte[1000 * 1000 * 3];
                new Random().NextBytes(bytearr);
                car.ByteArray = bytearr;
                car.SaveToFile("out.png");

                var nc = new ImageCarrier("out.png");
                File.WriteAllBytes("original.bin", bytearr);
                File.WriteAllBytes("new.bin", nc.ByteArray);
                Assert.IsTrue(bytearr.SequenceEqual(nc.ByteArray));
            }

        }
    }
}
