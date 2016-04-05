using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Extensions
{
    public static class ByteListExtensions
    {
        /// <summary>
        /// Insert a string into the list of bytes
        /// </summary>
        public static void AddString(this List<byte> _byteList, string _str)
        {
            byte[] encodedString = Encoding.UTF8.GetBytes(_str);
            _byteList.AddRange(BitConverter.GetBytes(encodedString.Length));
            _byteList.AddRange(encodedString);
        }

        /// <summary>
        /// Adds Int32 to list of bytes
        /// </summary>
        public static void AddInt(this List<byte> _byteList, int _int)
        {
            _byteList.AddRange(BitConverter.GetBytes(_int));
        }

        /// <summary>
        /// Adds Int16 to list of bytes
        /// </summary>
        public static void AddShort(this List<byte> _byteList, short _short)
        {
            _byteList.AddRange(BitConverter.GetBytes(_short));
        }

        /// <summary>
        /// Returns integer from list of bytes at specified startIndex
        /// </summary>
        public static int ReadInt(this List<byte> _byteList, int _startIndex)
        {
            return BitConverter.ToInt32(_byteList.ReadBytes(_startIndex, sizeof(int)), 0);
        }

        /// <summary>
        /// Returns short from list of bytes at specified startIndex
        /// </summary>
        public static short ReadShort(this List<byte> _byteList, int _startIndex)
        {
            return BitConverter.ToInt16(_byteList.ReadBytes(_startIndex, sizeof(short)), 0);
        }

        /// <summary>
        /// Reads and returns byte array
        /// </summary>
        /// <param name="_byteList">List of bytes</param>
        /// <param name="_startIndex">Starting point in list</param>
        /// <param name="_count">Number of bytes to return</param>
        public static byte[] ReadBytes(this List<byte> _byteList, int _startIndex, int _count)
        {
            return _byteList.GetRange(_startIndex, _count).ToArray();
        }

    }
}
