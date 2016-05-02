using System.Linq;
using Stegosaurus.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StegosaurusTest
{
    [TestClass]
    public class AESTests
    {
        AESProvider AESCryptation = new AESProvider();

        byte[] input = new byte[] { 39, 0, 0, 0, 31, 139, 8, 0, 0, 0, 0, 0, 4, 0, 99, 100, 96,
                                        96, 96, 7, 226, 220, 196, 18, 189, 130, 188, 116, 14,
                                        6, 56, 96, 4, 17, 0, 173, 73, 103, 222, 31, 0, 0, 0 };

        [TestMethod]
        public void Encrypt_MatchingArrays_ReturnsFalse()
        {
            AESCryptation.CryptoKey = "KeyKeyKey";

            byte[] encryptedArray = AESCryptation.Encrypt(input);

            // Tests if the encrypted array is not the same as the inout array
            Assert.IsFalse(encryptedArray.SequenceEqual(input));
        }

        [TestMethod]
        public void Decrypt_MatchingArrays_ReturnsTrue()
        {
            AESCryptation.CryptoKey = "KeyKeyKey";

            byte[] decryptedArray = AESCryptation.Decrypt(AESCryptation.Encrypt(input));

            //Tests if the decrypted array is the same as input array
            Assert.IsTrue(decryptedArray.SequenceEqual(input));
        }
    }
}
