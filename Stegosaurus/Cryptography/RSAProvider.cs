using Stegosaurus.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Stegosaurus.Cryptography
{
    public class RSAProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }
        public string Name => "RSA";

        public int Seed => 0;

        private const int KeySize = 2048;

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

                return rsaProvider.Decrypt(_data, false);
            }
        }

        public byte[] Encrypt(byte[] _data)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(KeySize))
            {
                rsaProvider.ImportParameters(Parameters);

                return rsaProvider.Encrypt(_data, false);
            }
        }

        public static void GenerateKeys()
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(KeySize))
            {
                RSAParameters publicKey = rsaProvider.ExportParameters(false);
                File.WriteAllText("public_key.xml", SerializeKey(publicKey));

                RSAParameters privateKey = rsaProvider.ExportParameters(true);
                File.WriteAllText("private_key.xml", SerializeKey(privateKey));
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
