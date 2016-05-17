using System.IO;
using System.IO.Compression;

namespace Stegosaurus.Utility
{
    public static class Compression
    {
        /// <summary>
        /// Returns compressed byte array from an existing byte array.
        /// Source: http://www.dotnetperls.com/compress
        /// </summary>
        public static byte[] Compress(byte[] _bytes)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(_bytes, 0, _bytes.Length);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// Returns a decompressed byte array from an existing byte array.
        /// Source: http://www.dotnetperls.com/decompress
        /// </summary>
        public static byte[] Decompress(byte[] _byteArray)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(_byteArray), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
