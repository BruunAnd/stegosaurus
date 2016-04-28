using System.IO;
using System.Security.Cryptography;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Cryptography
{
    public class AESProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }
        public string Name => "AES";

        private static byte[] salt = { 0x4e, 0x27, 0xaa, 0x18, 0x8b, 0xf7, 0x7f, 0x76 };

        public byte[] Encrypt(byte[] _data)
        {
            using (AesManaged aesAlgorithm = new AesManaged())
            {
                // Generate key from key and salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(CryptoKey, salt);

                // Set key and generate initialization vector
                aesAlgorithm.Key = key.GetBytes(aesAlgorithm.KeySize / 8);
                aesAlgorithm.GenerateIV();

                // Create encryptor
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                using (MemoryStream outputStream = new MemoryStream())
                {
                    if (aesAlgorithm.IV.Length != 16)
                        throw new StegosaurusException("Unexpected length of initialization vector."); // TODO custom exception

                    // Append initialization vector to stream
                    outputStream.Write(aesAlgorithm.IV);

                    using (CryptoStream cryptStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptStream.Write(_data);
                    }

                    return outputStream.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] _data)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                // Generate key from key and salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(CryptoKey, salt);

                // Set key and generate initialization vector
                aesManaged.Key = key.GetBytes(aesManaged.KeySize / 8);

                using (MemoryStream inputStream = new MemoryStream(_data))
                {
                    // Read initialization vector from stream
                    aesManaged.IV = inputStream.ReadBytes(16);

                    // Create decryptor
                    ICryptoTransform decryptor = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV);

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
