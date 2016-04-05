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
            byte[] intRange = _byteList.GetRange(_startIndex, sizeof (int)).ToArray();
            return BitConverter.ToInt32(intRange, 0);
        }

        /// <summary>
        /// Returns short from list of bytes at specified startIndex
        /// </summary>
        public static short ReadShort(this List<byte> _byteList, int _startIndex)
        {
            byte[] shortRange = _byteList.GetRange(_startIndex, sizeof(short)).ToArray();
            return BitConverter.ToInt16(shortRange, 0);
        }

    }
}
