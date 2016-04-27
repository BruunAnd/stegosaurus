using System.IO;
using System.Security.Cryptography;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Cryptography
{
    public class AESProvider : ICryptoProvider
    {
        private static byte[] salt = new byte[] { 0x4e, 0x27, 0xaa, 0x18, 0x8b, 0xf7, 0x7f, 0x76 };

        public byte[] Encrypt(byte[] _data, string _key)
        {
            using (RijndaelManaged rijnAlgo = new RijndaelManaged())
            {
                // Generate key from key and salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_key, salt);

                // Set key and generate initialization vector
                rijnAlgo.Key = key.GetBytes(rijnAlgo.KeySize / 8);
                rijnAlgo.GenerateIV();

                // Create encryptor
                ICryptoTransform encryptor = rijnAlgo.CreateEncryptor(rijnAlgo.Key, rijnAlgo.IV);

                using (MemoryStream outputStream = new MemoryStream())
                {
                    // Append initialization vector to stream
                    outputStream.Write(rijnAlgo.IV);

                    using (CryptoStream cryptStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptStream.Write(_data);
                    }

                    return outputStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] _data, string _key)
        {
            using (RijndaelManaged rijnAlgo = new RijndaelManaged())
            {
                // Generate key from key and salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(_key, salt);

                // Set key and generate initialization vector
                rijnAlgo.Key = key.GetBytes(rijnAlgo.KeySize / 8);

                using (MemoryStream inputStream = new MemoryStream(_data))
                {
                    // Read initialization vector from stream
                    rijnAlgo.IV = inputStream.ReadBytes(16);

                    // Create decryptor
                    ICryptoTransform decryptor = rijnAlgo.CreateDecryptor(rijnAlgo.Key, rijnAlgo.IV);

                    using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(outputStream);

                            return outputStream.ToArray();
                        }
                    }
                }
            }
        }

    }
}
