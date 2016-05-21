using System.IO;
using System.IO.Compression;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Utility
{
    public static class Compression
    {
        /// <summary>
        /// Returns compressed byte array from an existing byte array.
        /// </summary>
        public static byte[] Compress(byte[] _bytes)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(outStream, CompressionLevel.Optimal))
                {
                    deflateStream.Write(_bytes);
                }

                return outStream.ToArray();
            }
        }

        /// <summary>
        /// Returns a decompressed byte array from an existing byte array.
        /// </summary>
        public static byte[] Decompress(byte[] _byteArray)
        {
            using (MemoryStream returnStream = new MemoryStream())
            {
                using (MemoryStream inStream = new MemoryStream(_byteArray))
                {
                    using (DeflateStream deflateStream = new DeflateStream(inStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(returnStream);
                        return returnStream.ToArray();
                    }
                }
            }
        }
    }
}
