using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Cryptography;
using System.Security.Cryptography;

namespace StegosaurusTest
{
    [TestClass]
    public class RSATests
    {
        [TestMethod]
        public void DecryptCorrectKey_SameOutput()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            RSAKeyPair keyPair = RSAProvider.GenerateKeys(2048);

            RSAProvider rsa = new RSAProvider();
            rsa.SetKey(keyPair.PublicKey);
            byte[] encryptedBytes = rsa.Encrypt(randomBytes);

            // use correct (corresponding private) key
            rsa.SetKey(keyPair.PrivateKey);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes);

            Assert.IsTrue(decryptedBytes.SequenceEqual(randomBytes));
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographicException))]
        public void DecryptWrongKey_ThrowsCryptographicException()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new RSAProvider();
            cryptoProvider.SetKey(RSAProvider.GenerateKeys(2048).PublicKey);
            byte[] encryptedData = cryptoProvider.Encrypt(randomBytes);
            // use a new private key, that should not correspond
            cryptoProvider.SetKey(RSAProvider.GenerateKeys(2048).PrivateKey);
            byte[] decryptedData = cryptoProvider.Decrypt(encryptedData);
        }
    }
}
