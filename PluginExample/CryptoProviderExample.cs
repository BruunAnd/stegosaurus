using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stegosaurus.Cryptography;

namespace PluginExample
{
    public class CryptoProviderExample : ICryptoProvider
    {
        public byte[] Key { get; set; }
        public int HeaderSize => 0;
        public string Name => "Unencrypted (Plugin)";
        public int KeySize => 0;
        public int Seed => 0;

        public void SetKey(string _keyString)
        {
        }

        public byte[] GenerateKey()
        {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(byte[] _data)
        {
            return _data;
        }

        public byte[] Decrypt(byte[] _data)
        {
            return _data;
        }
    }
}
