using System.Security.Cryptography;
using System.Text;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Utility
{
    public static class KeyDeriver
    {
        /// <summary>
        /// Returns a key with a correct length, using the specified key string.
        /// </summary>
        public static byte[] DeriveKey(string _key, int _keySize)
        {
            byte[] salt = Encoding.UTF8.GetBytes(_key).ComputeSHAHash();
            return new Rfc2898DeriveBytes(_key, salt).GetBytes(_keySize / 8);
        }
    }
}
