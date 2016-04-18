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
            if (string.IsNullOrEmpty(_str))
            {
                _byteList.AddInt(0);
            }
            else
            {
                byte[] encodedString = Encoding.UTF8.GetBytes(_str);
                _byteList.AddRange(BitConverter.GetBytes(encodedString.Length));
                _byteList.AddRange(encodedString);
            }
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


    }
}
