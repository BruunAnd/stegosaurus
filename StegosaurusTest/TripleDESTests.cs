using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using Stegosaurus.Cryptography;
using System.Linq;

namespace StegosaurusTest.CryptographyTests
{
    [TestClass]
    public class TripleDESTests
    {
        [TestMethod]
        public void TestEncryptDecryptSameKey()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new TripleDESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomBytes);
            byte[] decryptedData = cryptoProvider.Decrypt(encryptedData);

            Assert.IsTrue(decryptedData.SequenceEqual(randomBytes));
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptWrongKey_ThrowsCryptographicException()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new TripleDESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomBytes);
            // change key
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] decryptedData = cryptoProvider.Decrypt(encryptedData);
        }
    }
}
