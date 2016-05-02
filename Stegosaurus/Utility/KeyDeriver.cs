using System.Security.Cryptography;

namespace Stegosaurus.Utility
{
    public static class KeyDeriver
    {
        private static byte[] salt = { 0x4e, 0x27, 0xaa, 0x18, 0x8b, 0xf7, 0x7f, 0x76 };

        public static byte[] DeriveKey(string _key, int _keySize)
        {
            return new Rfc2898DeriveBytes(_key, salt).GetBytes(_keySize / 8);
        }
    }
}
