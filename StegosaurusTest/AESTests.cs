using System.Linq;
using Stegosaurus.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;

namespace StegosaurusTest
{
    [TestClass]
    public class AESTests
    {
        [TestMethod]
        public void DecryptSameKey_SameOutput()
        {
            byte[] randomData = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new AESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomData);
            // don't change the key
            byte[] decryptedData = cryptoProvider.Decrypt(encryptedData);

            Assert.IsTrue(decryptedData.SequenceEqual(randomData));
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptWrongKey_ThrowsCryptographicException()
        {
            byte[] randomData = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new AESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomData);
            // generate new key
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] decryptedData = cryptoProvider.Decrypt(encryptedData);
        }
    }
}
