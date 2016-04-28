using System;
using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Cryptography;
using Stegosaurus.Utility;
using System.Linq;

namespace Stegosaurus
{
    public class StegoMessage
    {
        public string TextMessage { get; set; }
        public List<InputFile> InputFiles { get; } = new List<InputFile>();

        [Flags]
        public enum FlagEnum
        {
            Encoded = 0x1,
            Compressed = 0x2,
            Encrypted = 0x4,
            Signed = 0x8
        }

        private FlagEnum flags = new FlagEnum();
        private byte flagByte;

        public StegoMessage()
        {
        }

        public StegoMessage(string _textMessage)
        {
            TextMessage = _textMessage;
        }

        public StegoMessage(byte[] _fromArray, byte[] _decryptionKey = null)
        {
            flagByte = _fromArray[0];
            flags = (FlagEnum)flagByte;

            _fromArray = _fromArray.Skip(1).ToArray();

            // Decrypt if a key is specified
            if (_decryptionKey != null && flags.HasFlag(FlagEnum.Encrypted))
            {
                _fromArray = RC4.Decrypt(_fromArray, _decryptionKey);
            }
                
            // Decode the decompressed array
            if(flags.HasFlag(FlagEnum.Compressed))
            {
                _fromArray = Compression.Decompress(_fromArray);
            }

            // Decode if data is encoded
            if(flags.HasFlag(FlagEnum.Encoded))
            {
                Decode(_fromArray);
            }
        }

        private void Decode(byte[] _byteArray)
        {
            using (MemoryStream tempStream = new MemoryStream(_byteArray))
            {
                // Read input files
                int numberOfFiles = tempStream.ReadInt();
                for (int i = 0; i < numberOfFiles; i++)
                    InputFiles.Add(tempStream.ReadInputFile());

                // Read text message
                TextMessage = tempStream.ReadString();
            }
        }

        private byte[] Encode()
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                // Write input files
                tempStream.Write(InputFiles.Count);
                foreach (InputFile inputFile in InputFiles)
                    tempStream.Write(inputFile);

                // Write text message
                tempStream.Write(TextMessage);

                return tempStream.ToArray();
            }
        }

        /// <summary>
        /// Converts text and/or file(s) into a byte array and combines them using a List.
        /// First part of the byte array contains the message file(s).
        /// The last part of the byte array is the text message if there is any.
        /// </summary>
        public byte[] ToByteArray(byte[] _encryptionKey = null)
        {
            // Encode and compress array
            byte[] encodedData = Encode();
            SetFlag(FlagEnum.Encoded, true);

            // Compress data
            byte[] compressedData = Compression.Compress(encodedData);
            if (compressedData.Length < encodedData.Length)
            {
                encodedData = compressedData;
                SetFlag(FlagEnum.Compressed, true);
            }
            else
            {
                SetFlag(FlagEnum.Compressed, false);
            }

            // Encrypt if key is specified
            if (_encryptionKey != null)
            {
                encodedData = RC4.Encrypt(encodedData, _encryptionKey);
                SetFlag(FlagEnum.Encrypted, true);
            }
            else
            {
                SetFlag(FlagEnum.Encrypted, false);
            }

            // Combine array and length header
            List<byte> returnList = new List<byte>();
            returnList.AddRange(BitConverter.GetBytes(encodedData.Length + sizeof(byte)));
            returnList.Add(flagByte);
            returnList.AddRange(encodedData);

            return returnList.ToArray();
        }

        private void SetFlag(FlagEnum _flag, bool _state)
        {
            if (_state != flags.HasFlag(_flag))
            {
                flagByte ^= (byte) _flag;
            }
        }

        public long GetCompressedSize()
        {
            return ToByteArray().Length;
        }
    }
}
