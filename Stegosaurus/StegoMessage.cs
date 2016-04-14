using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Stegosaurus
{
    /// <summary>
    /// The Class StegoMessage:
    /// - Converts message files and message text to bytearray
    /// - Compresses
    /// - Gets size of message and files after compression
    /// - Encrypts
    /// - Decrypts
    /// - Decompresses
    /// </summary>
    public class StegoMessage
    {
        public string TextMessage { get; private set; } = null;
        public byte[] EncryptionKey { get; private set; }
        public List<InputFile> InputFiles { get; private set; } = null;
        
        //public byte[] Bytes { get; private set; } = null;
        //public List<byte> Bytes { get; private set; }

        /// <summary>
        /// Sets the properties "TextMessage" and "InputFiles".
        /// Calls method: ToByteArray()
        /// </summary>
        /// <param name="_textMessage"></param>
        /// <param name="_inputFiles"></param>
        /// <param name="_encryptionKey"></param>

        public StegoMessage(string _encryptionKey, string _textMessage, List<InputFile>_inputFiles)
        {
            if (_textMessage != null)
            {
                TextMessage = _textMessage;
            }
            if (_inputFiles != null)
            {
                InputFiles = _inputFiles;
            }
            if (_encryptionKey != null)
            {
                EncryptionKey = Encoding.UTF8.GetBytes(_encryptionKey);
            }
            
            ToByteArray();
        }
        public StegoMessage(string _encryptionKey, string _textMessage) : this(_encryptionKey, _textMessage, null) { }
        public StegoMessage(string _encryptionKey, List<InputFile>_inputFiles) : this(_encryptionKey, null, _inputFiles){ }
        
        /// <summary>
        /// Converts text and/or file(s) into a byte array and combines them using a List.
        /// First part of the byte array contains the message file(s).
        /// The last part of the byte array is the text message if there is any.
        /// </summary>
        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();

            // Iterates over all files in InputFiles (and the number of files), or a TextMessage, 
            // and converts these to bytes and saves them to byteList.
            // The lenght of the data is store  before the data itself, to ease future extraction.
            if (InputFiles != null)
            {
                int numberOfFiles = InputFiles.Count;
                byteList.AddRange(BitConverter.GetBytes(numberOfFiles));
                foreach (InputFile file in InputFiles)
                {
                    byteList.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetBytes(file.Name).Length));
                    byteList.AddRange(Encoding.UTF8.GetBytes(file.Name));
                }
            }
            if (TextMessage != null)
            {
                byteList.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetBytes(TextMessage).Length));
                byteList.AddRange(Encoding.UTF8.GetBytes(TextMessage));
            }

            byte[] compressedArray = Compress(byteList.ToArray());
            byteList.Clear();
            byteList.AddRange(BitConverter.GetBytes(compressedArray.Length));
            byteList.AddRange(compressedArray);

            return byteList.ToArray();
        }
        /// <summary>
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
        private byte[] Encrypt(byte[] _data)
        {
            int a, i, j, k, tmp, maxKeySize = 256;
            int[] key, box;
            byte[] cipher;

            key = new int[maxKeySize];
            box = new int[maxKeySize];
            cipher = new byte[_data.Length];

            for (i = 0; i < maxKeySize; i++)
            {
                key[i] = EncryptionKey[i % EncryptionKey.Length];
                box[i] = i;
            }
            for (j = i = 0; i < maxKeySize; i++)
            {
                j = (j + box[i] + key[i]) % maxKeySize;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
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

        private byte[] Decrypt(byte[] _data)
        {
            return Encrypt(_data);
        }

        private StegoMessage Decompress(byte[] _byteArray)
        {
            return this;
        }
        private long GetCompressedSize()
        {
            return 1;
        }
    }
}
