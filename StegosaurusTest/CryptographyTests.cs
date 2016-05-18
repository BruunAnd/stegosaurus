using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Stegosaurus.Cryptography;
using System.Security.Cryptography;

namespace StegosaurusTest
{
    [TestClass]
    public class CryptographyTests
    {
        [TestMethod]
        public void TripleDES_DecryptCorrectKey_CorrectOutput ()
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
        public void TripleDES_DecryptWrongKey_ThrowsCryptographicException ()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new TripleDESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomBytes);
            // change key
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            cryptoProvider.Decrypt(encryptedData);
        }

        [TestMethod]
        public void RSA_DecryptCorrectKey_CorrectOutput ()
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
        public void RSA_DecryptWrongKey_ThrowsCryptographicException ()
        {
            byte[] randomBytes = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new RSAProvider();
            cryptoProvider.SetKey(RSAProvider.GenerateKeys(2048).PublicKey);
            byte[] encryptedData = cryptoProvider.Encrypt(randomBytes);
            // use a new private key, that should not correspond
            cryptoProvider.SetKey(RSAProvider.GenerateKeys(2048).PrivateKey);
            cryptoProvider.Decrypt(encryptedData);
        }
        
        [TestMethod]
        public void AES_DecryptCorrectKey_CorrectOutput ()
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
        public void AES_DecryptWrongKey_ThrowsCryptographicException ()
        {
            byte[] randomData = TestUtility.GetRandomBytes(32 * 1024);

            ICryptoProvider cryptoProvider = new AESProvider();
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            byte[] encryptedData = cryptoProvider.Encrypt(randomData);
            // generate new key
            cryptoProvider.Key = cryptoProvider.GenerateKey();
            cryptoProvider.Decrypt(encryptedData);
        }
    }
}
