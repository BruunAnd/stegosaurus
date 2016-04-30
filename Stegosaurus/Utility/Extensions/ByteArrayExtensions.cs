using System.Security.Cryptography;

namespace Stegosaurus.Utility.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int ComputeHash(this byte[] _array)
        {
            int hash = 17;

            foreach (char value in _array)
                hash = hash * 23 + value.GetHashCode();

            return hash;
        }

        public static byte[] ComputeSHAHash(this byte[] _array)
        {
            using (SHA1Managed sha = new SHA1Managed())
            {
                return sha.ComputeHash(_array);
            }
        }
    }
}
