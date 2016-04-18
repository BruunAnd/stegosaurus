using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Stegosaurus;

namespace StegosaurusTest
{
    [TestClass]
    public class UnitTest1
    {
        //List<InputFile> InputFiles = new List<InputFile>();
        //InputFiles.Add = 
        //String TextMessage = "Input testing, Converts to bytes";

        //[TestMethod]
        //public void ToByteArray_ConvertToByte_True()
        //{
        //    List<byte> byteList = new List<byte>();

        //    // Iterates over all files in InputFiles (and the number of files), or a TextMessage, 
        //    // and converts these to bytes and saves them to byteList.
        //    // The lenght of the data is store  before the data itself, to ease future extraction.
        //    if (InputFiles != null)
        //    {
        //        int numberOfFiles = InputFiles.Count;
        //        byteList.AddRange(BitConverter.GetBytes(numberOfFiles));
        //        foreach (InputFile file in InputFiles)
        //        {
        //            byteList.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetBytes(file.Name).Length));
        //            byteList.AddRange(Encoding.UTF8.GetBytes(file.Name));
        //        }
        //    }
        //    if (TextMessage != null)
        //    {
        //        byteList.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetBytes(TextMessage).Length));
        //        byteList.AddRange(Encoding.UTF8.GetBytes(TextMessage));
        //    }

        //    //byte[] compressedArray = Compress(byteList.ToArray());
        //    //byteList.Clear();
        //    //byteList.AddRange(BitConverter.GetBytes(compressedArray.Length));
        //    //byteList.AddRange(compressedArray);

        //}
        //[SetUp]
        //public void SetupTest()
        //{
            
        //}
    }
}
