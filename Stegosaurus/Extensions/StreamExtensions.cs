﻿using System;
using System.IO;
using System.Text;

namespace Stegosaurus.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Read and return bytes from stream
        /// </summary>
        public static byte[] ReadBytes(this Stream _stream, int _count)
        {
            byte[] buffer = new byte[_count];
            _stream.Read(buffer, 0, _count);
            return buffer;
        }

        /// <summary>
        /// Read and return int from stream
        /// </summary>
        public static int ReadInt(this Stream _stream)
        {
            return BitConverter.ToInt32(_stream.ReadBytes(sizeof(int)), 0);
        }

        /// <summary>
        /// Read and return short from stream
        /// </summary>
        public static short ReadShort(this Stream _stream)
        {
            return BitConverter.ToInt16(_stream.ReadBytes(sizeof(short)), 0);
        }

        /// <summary>
        /// Read and return string from stream
        /// </summary>
        public static string ReadString(this Stream _stream)
        {
            int stringLength = _stream.ReadInt();
            if (stringLength == 0)
                return null;
            else
                return Encoding.UTF8.GetString(_stream.ReadBytes(stringLength));
        }

        /// <summary>
        /// Read and return InputFile from stream
        /// </summary>
        public static InputFile ReadInputFile(this Stream _stream)
        {
            string fileName = _stream.ReadString();
            int lengthOfContent = _stream.ReadInt();
            byte[] content = _stream.ReadBytes(lengthOfContent);
            return new InputFile(_stream.ReadString(), _stream.ReadBytes(_stream.ReadInt()));
        }
    }
}
