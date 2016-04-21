using System;
using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Cryptography;
using Stegosaurus.Utility;

namespace Stegosaurus
{
    public class StegoMessage
    {
        public string TextMessage { get; set; }
        public List<InputFile> InputFiles { get; } = new List<InputFile>();

        public StegoMessage()
        {
        }

        public StegoMessage(string _textMessage)
        {
            TextMessage = _textMessage;
        }

        public StegoMessage(byte[] _fromArray, byte[] _decryptionKey = null)
        {
            // Decrypt if a key is specified
            if (_decryptionKey != null)
                _fromArray = RC4.Decrypt(_fromArray, _decryptionKey);

            // Decode the decompressed array
            Decode(Compression.Decompress(_fromArray));
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
            byte[] compressedArray = Compression.Compress(Encode());

            // Encrypt if key is specified
            if (_encryptionKey != null)
                compressedArray = RC4.Encrypt(compressedArray, _encryptionKey);

            // Combine array and length header
            List<byte> returnList = new List<byte>();
            returnList.AddRange(BitConverter.GetBytes(compressedArray.Length));
            returnList.AddRange(compressedArray);

            return returnList.ToArray();
        }

        private long GetCompressedSize()
        {
            return ToByteArray().Length;
        }
    }
}
