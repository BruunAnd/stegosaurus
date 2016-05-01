using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Cryptography
{
    public class RSAProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }
        public string Name => "RSA";

        /// <summary>
        /// The same seed is needed for encryption and decryption.
        /// Since the public and private key share modulus, we use its hash as a seed.
        /// </summary>
        public int Seed => string.IsNullOrEmpty(CryptoKey) ? 0 : Parameters.Modulus.ComputeHash();
        public int KeySize => 2048;

        private RSAParameters Parameters
        {
            get
            {
                StringReader stringReader = new StringReader(CryptoKey);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
                RSAParameters key = (RSAParameters) xmlSerializer.Deserialize(stringReader);

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
                    byte[] tripleDesKey = rsaProvider.Decrypt(tempStream.ReadBytes(), true);

                    TripleDESProvider des = new TripleDESProvider();
                    des.OverriddenKey = tripleDesKey;

                    return des.Decrypt(tempStream.ReadBytes((int) tempStream.GetRemainingLength()));
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
                    TripleDESProvider des = new TripleDESProvider();
                    des.OverriddenKey = des.GenerateKey();

                    // Write the encrypted key and its length
                    tempStream.Write(rsaProvider.Encrypt(des.OverriddenKey, false), true);
                    // Encrypt and write the main data
                    tempStream.Write(des.Encrypt(_data));

                    return tempStream.ToArray();
                }
            }
        }

        public static RSAKeyPair GenerateKeys(int _keySize)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(_keySize))
            {
                RSAKeyPair newKeyPair = new RSAKeyPair();
                newKeyPair.PublicKey = SerializeKey(rsaProvider.ExportParameters(false));
                newKeyPair.PrivateKey = SerializeKey(rsaProvider.ExportParameters(true));

                return newKeyPair;
            }
        }

        private static string SerializeKey(RSAParameters key)
        {
            var stringWriter = new StringWriter();
            var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, key);

            return stringWriter.ToString();
        }
    }
}
