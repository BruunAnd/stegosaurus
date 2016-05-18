using Stegosaurus.Utility;
using Stegosaurus.Utility.Extensions;
using System.Security.Cryptography;

namespace Stegosaurus.Cryptography
{
    public class TripleDESProvider : ICryptoProvider
    {
        public string Name => "TripleDES";

        public int Seed => Key?.ComputeHash() ?? 0;
        public int KeySize => 192;

        public byte[] Key { get; set; }

        public byte[] Decrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.KeySize = KeySize;
                des.Mode = CipherMode.ECB;
                des.Key = Key;

                ICryptoTransform desTransform = des.CreateDecryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }

        public byte[] Encrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Key = Key;

                ICryptoTransform desTransform = des.CreateEncryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }

        public byte[] GenerateKey()
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.KeySize = KeySize;
                des.GenerateKey();
                return des.Key;
            }
        }

        public void SetKey(string _keyString)
        {
            Key = string.IsNullOrEmpty(_keyString) ? null : KeyDeriver.DeriveKey(_keyString, KeySize);
        }
    }
}
