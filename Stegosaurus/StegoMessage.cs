using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using Stegosaurus.Extensions;
using System.Linq;
using Stegosaurus.Exceptions;

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
                _fromArray = Encrypt(_fromArray, _decryptionKey);

            // Decode the decompressed array
            Decode(Decompress(_fromArray));
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
            byte[] compressedArray = Compress(Encode());

            // Encrypt if key is specified
            if (_encryptionKey != null)
                compressedArray = Encrypt(compressedArray, _encryptionKey);

            // Combine array and length header
            List<byte> returnList = new List<byte>();
            returnList.AddRange(BitConverter.GetBytes(compressedArray.Length));
            returnList.AddRange(compressedArray);

            return returnList.ToArray();
        }

        /// <summary>
        /// http://www.dotnetperls.com/compress
        /// Gets input from Bytes[] and compresses the data, and returns it to Bytes[] in compressed form.
        /// </summary>
        /// <returns></returns>
        private byte[] Compress(byte[] _bytes)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(_bytes, 0, _bytes.Length);
                }

                return memory.ToArray();
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/7217627/is-there-anything-wrong-with-this-rc4-encryption-code-in-c-sharp
        /// </summary>
        /// <param name="_data">Data to encrypt</param>
        /// <returns>Returns the cypher(the encrypted byte array)</returns>
        private byte[] Encrypt(byte[] _data, byte[] _key)
        {
            int a, i, j, k, tmp, maxKeySize = 256;
            int[] key, box;
            byte[] cipher;

            key = new int[maxKeySize];
            box = new int[maxKeySize];
            cipher = new byte[_data.Length];

            // Copies EncrytionKey, into int[] Key, byte by byte, and repeats the process up to maxKeySize,  to ensure the same key lenght.
            for (i = 0; i < maxKeySize; i++)
            {
                key[i] = _key[i % _key.Length];
                box[i] = i;
            }
            // Swaps data elements in int[] box, by exchanging int[i] with int[j].
            for (j = i = 0; i < maxKeySize; i++)
            {
                j = (j + box[i] + key[i]) % maxKeySize;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            // Further swaps elements in int[] box, and assigns them to byte[] cipher which is returned to method call.
            for (a = j = i = 0; i < _data.Length; i++)
            {
                a++;
                a %= maxKeySize;
                j += box[a];
                j %= maxKeySize;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[((box[a] + box[j]) % maxKeySize)];
                cipher[i] = (byte)(_data[i] ^ k);
            }
            return cipher;
        }
        
        /// <summary>
        /// http://www.dotnetperls.com/decompress
        /// TODO read up on CompressionMode and GZipStream
        /// </summary>
        private byte[] Decompress(byte[] _byteArray)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(_byteArray), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        private long GetCompressedSize()
        {
            return ToByteArray().Length;
        }
    }
}
