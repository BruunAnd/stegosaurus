using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stegosaurus.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadBytes(this Stream _stream, int _count)
        {
            byte[] buffer = new byte[_count];
            _stream.Read(buffer, 0, _count);
            return buffer;
        }

        public static int ReadInt(this Stream _stream)
        {
            return BitConverter.ToInt32(_stream.ReadBytes(sizeof(int)), 0);
        }

        public static short ReadShort(this Stream _stream)
        {
            return BitConverter.ToInt16(_stream.ReadBytes(sizeof(short)), 0);
        }
    }
}
