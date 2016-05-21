using System.IO;
using System.Security.Cryptography;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;

namespace Stegosaurus.Cryptography
{
    public class AESProvider : ICryptoProvider
    {
        public string Name => "AES";

        public int Seed => Key?.ComputeHash() ?? 0;
        public int HeaderSize => Key == null ? 0 : 16;
        public int KeySize => 256;

        public byte[] Key { get; set; }

        public byte[] Encrypt(byte[] _data)
        {
            using (AesManaged aesAlgorithm = new AesManaged())
            {
                aesAlgorithm.KeySize = KeySize;

                // Set key and generate initialization vector
                aesAlgorithm.Key = Key;
                aesAlgorithm.GenerateIV();

                // Create encryptor
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

                using (MemoryStream outputStream = new MemoryStream())
                {
                    if (aesAlgorithm.IV.Length != 16)
                    {
                        throw new StegoCryptoException("Unexpected length of initialization vector.");
                    }

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
                aesManaged.Key = Key;

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

        public byte[] GenerateKey()
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.KeySize = KeySize;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public void SetKey(string _keyString)
        {
            Key = string.IsNullOrEmpty(_keyString) ? null : KeyDeriver.DeriveKey(_keyString, KeySize);
        }
    }
}
