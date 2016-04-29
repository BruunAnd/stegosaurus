using System.IO;
using System.Security.Cryptography;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;

namespace Stegosaurus.Cryptography
{
    public class AESProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }
        public string Name => "AES";

        public int Seed => KeyDeriver.DeriveKey(CryptoKey, KeySize).ComputeHash();

        private const int KeySize = 256;

        public byte[] Encrypt(byte[] _data)
        {
            using (AesManaged aesAlgorithm = new AesManaged())
            {
                aesAlgorithm.KeySize = 256;

                // Set key and generate initialization vector
                aesAlgorithm.Key = KeyDeriver.DeriveKey(CryptoKey, aesAlgorithm.KeySize);
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
                aesManaged.Key = KeyDeriver.DeriveKey(CryptoKey, aesManaged.KeySize);

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
