using Stegosaurus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Stegosaurus.Cryptography
{
    public class TripleDESProvider : ICryptoProvider
    {
        public string CryptoKey { get; set; }

        public string Name => "TripleDES";

        public byte[] Decrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Key = KeyDeriver.DeriveKey(CryptoKey, des.KeySize);

                ICryptoTransform desTransform = des.CreateDecryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }

        public byte[] Encrypt(byte[] _data)
        {
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Key = KeyDeriver.DeriveKey(CryptoKey, des.KeySize);

                ICryptoTransform desTransform = des.CreateEncryptor();
                return desTransform.TransformFinalBlock(_data, 0, _data.Length);
            }
        }
    }
}
