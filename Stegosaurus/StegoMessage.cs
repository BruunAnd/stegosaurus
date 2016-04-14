using System.Collections.Generic;

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
        public byte[] Bytes { get; private set; } = null;
        //public List<byte> Bytes { get; private set; }

        /// <summary>
        /// Sets the properties "TextMessage" and "InputFiles".
        /// Calls method: ToByteArray()
        /// </summary>
        /// <param name="_textMessage"></param>
        /// <param name="_inputFiles"></param>
        public StegoMessage(string _textMessage, List<InputFile>_inputFiles)
        {
            if (_textMessage != null)
            {
                TextMessage = _textMessage;
            }
            if (_inputFiles != null)
            {
                InputFiles = _inputFiles;
            }
            Bytes = ToByteArray();
        }
        public StegoMessage(string _textMessage) : this(_textMessage, null){ }
        public StegoMessage(List<InputFile>_inputFiles) : this(null, _inputFiles){ }
        
        /// <summary>
        /// Converts text and/or file(s) to a byte array and combines them using a List.
        /// First part of the byte array contains the message file(s).
        /// The last part of the byte array is the text message if there is any.
        /// </summary>
        private byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();

            if (InputFiles != null)
            {
                foreach (InputFile file in InputFiles)
                {
                    System.Text.Encoding.UTF8.GetBytes(file.Name, 0, file.Name.Length, byteList.ToArray(), byteList.Count);
                }
            }
            if (TextMessage != null)
            {
                byteList.AddRange(System.Text.Encoding.UTF8.GetBytes(TextMessage.ToCharArray()));
                byteList.Add('');
            }
            
            return byteList.ToArray();
        }

        private byte[] Compress()
        {

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
