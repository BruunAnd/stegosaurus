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
        public List<InputFile> InputFiles { get; private set; } = null;

        //public byte[] Bytes { get; private set; } = null;
        //public List<byte> Bytes { get; private set; }

        /// <summary>
        /// Sets the properties "TextMessage" and "InputFiles".
        /// Calls method: ToByteArray()
        /// </summary>
        /// <param name="_textMessage"></param>
        /// <param name="_inputFiles"></param>
        /// <param name="_encrypt"></param>

        public StegoMessage(bool _encrypt, string _textMessage, List<InputFile>_inputFiles)
        {
            if (_textMessage != null)
            {
                TextMessage = _textMessage;
            }
            if (_inputFiles != null)
            {
                InputFiles = _inputFiles;
            }
            ToByteArray(_encrypt);
        }
        public StegoMessage(bool _encrypt, string _textMessage) : this(_encrypt, _textMessage, null) { }
        public StegoMessage(bool _encrypt, List<InputFile>_inputFiles) : this(_encrypt, null, _inputFiles){ }
        
        /// <summary>
        /// Converts text and/or file(s) into a byte array and combines them using a List.
        /// First part of the byte array contains the message file(s).
        /// The last part of the byte array is the text message if there is any.
        /// </summary>
        private byte[] ToByteArray(bool _encrypt)
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
        private byte[] Encrypt(byte[] _byteArray)
        {

        }
        
        private byte[] Decrypt(byte[] _byteArray)
        {

        }
        private StegoMessage Decompress(byte[] _byteArray)
        {

        }
        private long GetCompressedSize()
        {
            return;
        }
    }
}
