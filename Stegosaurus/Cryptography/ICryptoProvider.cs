using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Cryptography
{
    public interface ICryptoProvider
    {
        string CryptoKey { get; set; }
        string Name { get; }
        int Seed { get; }

        byte[] Encrypt(byte[] _data);
        byte[] Decrypt(byte[] _data);
    }
}
