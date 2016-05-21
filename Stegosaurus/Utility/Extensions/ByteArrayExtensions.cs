using System.Security.Cryptography;

namespace Stegosaurus.Utility.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Returns an integer hash from the specified byte array.
        /// </summary>
        public static int ComputeHash(this byte[] _array)
        {
            int hash = 17;

            foreach (byte value in _array)
            {
                hash = hash * 23 + value;
            }

            return hash;
        }

        /// <summary>
        /// Returns a SHA hash (as a byte array) from an existing byte array.
        /// </summary>
        public static byte[] ComputeSHAHash(this byte[] _array)
        {
            using (SHA1Managed sha = new SHA1Managed())
            {
                return sha.ComputeHash(_array);
            }
        }
    }
}
