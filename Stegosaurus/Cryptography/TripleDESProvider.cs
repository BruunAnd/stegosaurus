using Stegosaurus.Utility;
using Stegosaurus.Utility.Extensions;
using System.Security.Cryptography;

namespace Stegosaurus.Cryptography
{
    public class TripleDESProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }

        public string Name => "TripleDES";

        public int Seed => string.IsNullOrEmpty(CryptoKey) ? 0 : KeyDeriver.DeriveKey(CryptoKey, KeySize).ComputeHash();
        public int KeySize => 192;

        public byte[] OverriddenKey { get; set; }

        public byte[] Decrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.KeySize = KeySize;
                des.Mode = CipherMode.ECB;
                des.Key = OverriddenKey ?? KeyDeriver.DeriveKey(CryptoKey, des.KeySize);

                ICryptoTransform desTransform = des.CreateDecryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }

        public byte[] Encrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Key = OverriddenKey ?? KeyDeriver.DeriveKey(CryptoKey, des.KeySize);

                ICryptoTransform desTransform = des.CreateEncryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }

        public byte[] GenerateKey()
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.GenerateKey();
                return des.Key;
            }
        }
    }
}
