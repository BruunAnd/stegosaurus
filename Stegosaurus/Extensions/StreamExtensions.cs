using System;
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
            return new InputFile(_stream.ReadString(), _stream.ReadBytes(_stream.ReadInt()));
        }

        /// <summary>
        /// Write buffer to stream
        /// </summary>
        public static void Write(this Stream _stream, byte[] _buffer)
        {
            _stream.Write(_buffer, 0, _buffer.Length);
        }

        /// <summary>
        /// Write int to stream
        /// </summary>
        public static void Write(this Stream _stream, int _value)
        {
            _stream.Write(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Write short to stream
        /// </summary>
        public static void Write(this Stream _stream, short _value)
        {
            _stream.Write(BitConverter.GetBytes(_value));
        }

        /// <summary>
        /// Write string to stream
        /// </summary>
        public static void Write(this Stream _stream, string _value)
        {
            if (string.IsNullOrEmpty(_value))
            {
                _stream.Write(0);
            }
            else
            {
                byte[] encodedString = Encoding.UTF8.GetBytes(_value);
                _stream.Write(encodedString.Length);
                _stream.Write(encodedString);
            }
        }

        /// <summary>
        /// Write InputFile to stream
        /// </summary>
        public static void Write(this Stream _stream, InputFile _value)
        {
            // Write name
            _stream.Write(_value.Name);

            // Write content with length
            _stream.Write(_value.Content.Length);
            _stream.Write(_value.Content);
        }
    }
}
