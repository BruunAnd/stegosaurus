using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Cryptography;

namespace StegosaurusTest
{
    [TestClass]
    public class RSATests
    {
        [TestMethod]
        public void TestRSAEncryptDecrypt()
        {
            byte[] randomBytes = new byte[1024 * 64];
            new Random().NextBytes(randomBytes);

            RSAKeyPair keyPair = RSAProvider.GenerateKeys(2048);

            // encrypt
            RSAProvider rsa = new RSAProvider();
            rsa.SetKey(keyPair.PublicKey);
            byte[] encryptedBytes = rsa.Encrypt(randomBytes);

            // decrypt
            rsa.SetKey(keyPair.PrivateKey);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes);

            // check if decrypted equals original
            Assert.IsTrue(decryptedBytes.SequenceEqual(randomBytes));
        }
    }
}
