using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Cryptography;
using System.Diagnostics;

namespace StegosaurusTest
{
    [TestClass]
    public class EncryptionTest
    {
        /*[TestMethod]
        public void Encryption_MatchingArrays_ReturnsTrue()
        {

            byte[] actualOutput = RC4.Encrypt(new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 83, 103, 96, 96, 144, 239, 230,
                                                           96, 0, 1, 22, 134, 228, 148, 132, 132, 4, 246, 71, 119, 142, 8,
                                                           237, 109, 218, 83, 194, 199, 102, 145, 192, 34, 200, 176, 214, 51,
                                                           253, 158, 60, 80, 26, 0, 130, 66, 240, 205, 43, 0, 0, 0 },
                                              new byte[] { 39, 0, 0, 0, 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 99,
                                                           100, 96, 96, 96, 7, 226, 220, 196, 18, 189, 130,
                                                           188, 116, 14, 6, 56, 96, 4, 17, 0, 173, 73, 103,
                                                           222, 31, 0, 0, 0 });

            byte[] expectedOutput = new byte[] { 136, 225, 215, 2, 135, 123, 20, 184, 6, 224, 124, 60, 157,
                                                 213, 92, 68, 63, 88, 122, 23, 197, 120, 162, 16, 218, 157,
                                                 5, 204, 3, 89, 21, 153, 234, 8, 21, 170, 154, 42, 32, 30,
                                                 75, 176, 45, 236, 176, 124, 5, 112, 63, 73, 118, 236, 224,
                                                 170, 9, 142, 14, 169, 239, 253 };

            string expectedString = "", actualString = "";
            foreach (byte item in expectedOutput)
            {
                expectedString += item.ToString() + ", ";
            }

            foreach (byte item in actualOutput)
            {
                actualString += item.ToString() + ", ";
            }

            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Decryption_MatchingArrays_ReturnsTrue()
        {
            byte[] key = { 39, 0, 0, 0, 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 99,
                           100, 96, 96, 96, 7, 226, 220, 196, 18, 189, 130,
                           188, 116, 14, 6, 56, 96, 4, 17, 0, 173, 73, 103,
                           222, 31, 0, 0, 0  };

            byte[] inputData = {31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 83, 103, 96, 96, 144, 239, 230,
                                96, 0, 1, 22, 134, 228, 148, 132, 132, 4, 246, 71, 119, 142, 8,
                                237, 109, 218, 83, 194, 199, 102, 145, 192, 34, 200, 176, 214, 51,
                                253, 158, 60, 80, 26, 0, 130, 66, 240, 205, 43, 0, 0, 0 };

            byte[] encryptedArray = RC4.Encrypt(inputData, key);

            byte[] decryptedArray = RC4.Decrypt(encryptedArray, key);

            string expectedString = "", actualString = "";
            foreach (byte item in inputData)
            {
                expectedString += item.ToString() + ", ";
            }

            foreach (byte item in decryptedArray)
            {
                actualString += item.ToString() + ", ";
            }

            Assert.AreEqual(expectedString, actualString);
        }*/
    }
}
