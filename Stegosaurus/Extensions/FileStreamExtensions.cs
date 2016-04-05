using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stegosaurus.Extensions
{
    public static class FileStreamExtensions
    {
        public static byte[] ReadBytes(this FileStream _fileStream, int _count)
        {
            byte[] buffer = new byte[_count];
            _fileStream.Read(buffer, 0, _count);
            return buffer;
        }

        public static int ReadInt(this FileStream _fileStream)
        {
            return BitConverter.ToInt32(_fileStream.ReadBytes(sizeof(int)), 0);
        }

        public static short ReadShort(this FileStream _fileStream)
        {
            return BitConverter.ToInt16(_fileStream.ReadBytes(sizeof(short)), 0);
        }
    }
}
