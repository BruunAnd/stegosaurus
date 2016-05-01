﻿using System;
using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Cryptography;

namespace Stegosaurus
{
    public class StegoMessage
    {
        [Flags]
        public enum StegoMessageFlags
        {
            Encoded = 0x1,
            Compressed = 0x2,
            Encrypted = 0x4,
            Signed = 0x8,
        }

        public string TextMessage { get; set; }
        public List<InputFile> InputFiles { get; } = new List<InputFile>();

        public StegoMessageFlags Flags;

        public StegoMessage()
        {
        }

        public StegoMessage(string _textMessage)
        {
            TextMessage = _textMessage;
        }

        public StegoMessage(byte[] _fromArray, ICryptoProvider _cryptoProvider = null)
        {
            using (MemoryStream inputStream = new MemoryStream(_fromArray))
            {
                Flags = (StegoMessageFlags) inputStream.ReadByte();

                // Read encoded data
                byte[] encodedData = inputStream.ReadBytes(_fromArray.Length - sizeof(byte));

                // Decrypt if a key is specified
                if (Flags.HasFlag(StegoMessageFlags.Encrypted) && _cryptoProvider != null)
                {
                    encodedData = _cryptoProvider.Decrypt(encodedData);
                }

                // Decompress the array
                if (Flags.HasFlag(StegoMessageFlags.Compressed))
                {
                    encodedData = Ionic.Zlib.ZlibStream.UncompressBuffer(encodedData);
                }

                // Decode array
                if (Flags.HasFlag(StegoMessageFlags.Encoded))
                {
                    Decode(encodedData);
                }
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
        public byte[] ToByteArray(ICryptoProvider _cryptoProvider = null)
        {
            // Encode and compress array
            byte[] encodedData = Encode();
            SetFlag(StegoMessageFlags.Encoded, true);

            // Compress data
            byte[] compressedData = Ionic.Zlib.ZlibStream.CompressBuffer(encodedData);
            if (compressedData.Length < encodedData.Length)
            {
                encodedData = compressedData;
                SetFlag(StegoMessageFlags.Compressed, true);
            }
            else
            {
                SetFlag(StegoMessageFlags.Compressed, false);
            }

            // Encrypt if key is specified
            if (_cryptoProvider != null && !string.IsNullOrEmpty(_cryptoProvider.CryptoKey))
            {
                encodedData = _cryptoProvider.Encrypt(encodedData);
                SetFlag(StegoMessageFlags.Encrypted, true);
            }
            else
            {
                SetFlag(StegoMessageFlags.Encrypted, false);
            }

            // Combine array and length header
            List<byte> returnList = new List<byte>();
            returnList.AddRange(BitConverter.GetBytes(encodedData.Length + sizeof(byte)));
            returnList.Add((byte)Flags);
            returnList.AddRange(encodedData);

            return returnList.ToArray();
        }

        private void SetFlag(StegoMessageFlags _flag, bool _state)
        {
            // Check if current flag state does not equal wanted state
            if (_state != Flags.HasFlag(_flag))
            {
                // Flip state
                Flags ^= _flag;
            }
        }

        public long GetCompressedSize()
        {
            return ToByteArray().Length;
        }
    }
}
