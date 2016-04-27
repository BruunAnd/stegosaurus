using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Cryptography
{
    public interface ICryptoProvider
    {
        byte[] Encrypt(byte[] _data, string _key);
        byte[] Decrypt(byte[] _data, string key);
    }
}
