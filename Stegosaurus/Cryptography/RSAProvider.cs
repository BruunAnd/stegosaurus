using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Stegosaurus.Utility.Extensions;
using System.Text;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Cryptography
{
    public class RSAProvider : ICryptoProvider
    {
        public string Name => "RSA";

        /// <summary>
        /// The same seed is needed for encryption and decryption.
        /// Since the public and private key share modulus, we use its hash as a seed.
        /// </summary>
        public int Seed => Key == null ? 0 : Parameters.Modulus.ComputeHash();
        public int KeySize => 2048;

        public byte[] Key { get; set; }

        private RSAParameters Parameters
        {
            get
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
                RSAParameters key;
                try
                {
                    key = (RSAParameters) xmlSerializer.Deserialize(new MemoryStream(Key));
                }
                catch (InvalidOperationException ex)
                {
                    throw new StegoCryptoException("Invalid RSA key.", ex);
                }

                return key;
            }
        }

        public byte[] Decrypt(byte[] _data)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaProvider.ImportParameters(Parameters);

                // Decrypt the TripleDES key first, then decrypt main data using TripleDES
                using (MemoryStream tempStream = new MemoryStream(_data))
                {
                    byte[] symmetricKey = rsaProvider.Decrypt(tempStream.ReadBytes(), true);

                    ICryptoProvider cryptoProvider = new TripleDESProvider();
                    cryptoProvider.Key = symmetricKey;

                    return cryptoProvider.Decrypt(tempStream.ReadBytes((int) tempStream.GetRemainingLength()));
                }
            }
        }

        public byte[] Encrypt(byte[] _data)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaProvider.ImportParameters(Parameters);

                using (MemoryStream tempStream = new MemoryStream())
                {
                    ICryptoProvider cryptoProvider = new TripleDESProvider();
                    cryptoProvider.Key = cryptoProvider.GenerateKey();

                    // Write the encrypted key and its length
                    tempStream.Write(rsaProvider.Encrypt(cryptoProvider.Key, true), true);
                    // Encrypt and write the main data
                    tempStream.Write(cryptoProvider.Encrypt(_data));

                    return tempStream.ToArray();
                }
            }
        }


        /// <summary>
        /// Sign data using a private key.
        /// </summary>
        public byte[] SignData(byte[] _originalData)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.ImportParameters(Parameters);

                try
                {
                    return rsaProvider.SignData(_originalData, new SHA1CryptoServiceProvider());
                }
                catch (CryptographicException ex)
                {
                    throw new StegoCryptoException("Could not sign data, private key is invalid.", ex);
                }
            }
        }

        /// <summary>
        /// Verify data using a public key.
        /// </summary>
        public bool VerifyData(byte[] _originalData, byte[] _signedData)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.ImportParameters(Parameters);

                try
                {
                    return rsaProvider.VerifyData(_originalData, new SHA1CryptoServiceProvider(), _signedData);
                }
                catch (CryptographicException ex)
                {
                    throw new StegoCryptoException("Could not verify data, public key is invalid.", ex);
                }
            }
        }

        /// <summary>
        /// Generate a valid RSA keypair.
        /// </summary>
        public static RSAKeyPair GenerateKeys(int _keySize)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(_keySize))
            {
                RSAKeyPair newKeyPair = new RSAKeyPair
                {
                    PublicKey = SerializeKey(rsaProvider.ExportParameters(false)),
                    PrivateKey = SerializeKey(rsaProvider.ExportParameters(true))
                };

                return newKeyPair;
            }
        }

        private static string SerializeKey(RSAParameters _key)
        {
            var stringWriter = new StringWriter();
            var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, _key);

            return stringWriter.ToString();
        }

        public void SetKey(string _keyString)
        {
            Key = string.IsNullOrEmpty(_keyString) ? null : Encoding.Unicode.GetBytes(_keyString);
        }

        public byte[] GenerateKey()
        {
            throw new NotImplementedException("Use the method GenerateKeys to generate RSA keys.");
        }
    }
}
