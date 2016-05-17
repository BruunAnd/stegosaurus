using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stegosaurus.Cryptography;
using Stegosaurus;
using System.Linq;
using Stegosaurus.Exceptions;

namespace StegosaurusTest
{
    [TestClass]
    public class SigningTest
    {
        public void SignAndVerify(string _publicKey, string _privateKey)
        {
            const string trustedAlias = "TrustedUser";

            // Add public key to known keys.
            PublicKeyList.Add(new SavedPublicKey(trustedAlias, _publicKey));

            // Create signed dummy message.
            StegoMessage foo = new StegoMessage();
            foo.TextMessage = "FooBar";
            foo.PrivateSigningKey = _privateKey;
            byte[] encoded = foo.ToByteArray();

            // Recreate message.
            StegoMessage bar = new StegoMessage(encoded.Skip(4).ToArray());

            // Verify signed
            Assert.AreEqual(bar.SignState, StegoMessage.StegoMessageSignState.SignedByKnown);
            Assert.AreEqual(trustedAlias, bar.SignedBy);
        }

        [TestMethod]
        public void Verify_Sign_MatchingKeyPair()
        {
            RSAProvider rsa = new RSAProvider();
            RSAKeyPair pair = RSAProvider.GenerateKeys(rsa.KeySize);
            SignAndVerify(pair.PublicKey, pair.PrivateKey);
        }

        [TestMethod]
        [ExpectedException(typeof(StegoCryptoException))]
        public void Verify_Sign_InvalidPrivateKey()
        {
            RSAProvider rsa = new RSAProvider();
            RSAKeyPair pair = RSAProvider.GenerateKeys(rsa.KeySize);
            SignAndVerify(pair.PublicKey, pair.PublicKey);
        }
    }
}
