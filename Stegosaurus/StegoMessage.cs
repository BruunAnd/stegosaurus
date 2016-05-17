using System;
using System.Collections.Generic;
using System.IO;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Cryptography;
using System.Linq;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using System.Security.Cryptography;

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

        public enum StegoMessageSignState
        {
            Unsigned,
            SignedByKnown,
            SignedByUnknown,
        }

        /// <summary>
        /// This is the text message that can be saved in the StegoMessage.
        /// </summary>
        public string TextMessage { get; set; }

        /// <summary>
        /// This is where each file will be stored in the StegoMessage.
        /// </summary>
        public List<InputFile> InputFiles { get; } = new List<InputFile>();

        /// <summary>
        /// Private Signing Key used to verify the authenticity of the sender.
        /// </summary>
        public string PrivateSigningKey { get; set; }

        /// <summary>
        /// Indicates who has signed this message, if any.
        /// </summary>
        public string SignedBy { get; private set; }

        /// <summary>
        /// Indicates whether the message has been signed or not.
        /// </summary>
        public StegoMessageSignState SignState { get; private set; }

        public StegoMessageFlags Flags;

        

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public StegoMessage ()
        {
        }

        /// <summary>
        /// This constructer is used if only a text message is applied.
        /// </summary>
        public StegoMessage (string _textMessage)
        {
            TextMessage = _textMessage;
        }

        /// <summary>
        /// This constructer takes a byte array containing the data to add to the StegoMessage.
        /// </summary>
        public StegoMessage(byte[] _fromArray, ICryptoProvider _cryptoProvider = null)
        {
            using (MemoryStream inputStream = new MemoryStream(_fromArray))
            {
                Flags = (StegoMessageFlags) inputStream.ReadByte();

                // Read encoded data.
                byte[] encodedData = inputStream.ReadBytes();
                byte[] hash = inputStream.ReadBytes();

                // Verify hash of encoded data.
                if (!encodedData.ComputeSHAHash().SequenceEqual(hash))
                {
                    throw new StegoMessageException("Hash does not match, data is corrupt.");
                }

                // Verify authenticity of signed sender.
                if (Flags.HasFlag(StegoMessageFlags.Signed))
                {
                    SignState = StegoMessageSignState.SignedByUnknown;
                    byte[] signedData = inputStream.ReadBytes();

                    RSAProvider rsa = new RSAProvider();
                    foreach (SavedPublicKey key in PublicKeyList.GetKeyList())
                    {
                        // Set the RSA key to the current public key
                        rsa.SetKey(key.Key);

                        // Check if hashed data matches
                        if (!rsa.VerifyData(encodedData, signedData))
                            continue;

                        // Set sign state and who signed it
                        SignState = StegoMessageSignState.SignedByKnown;
                        SignedBy = key.Alias;
                    }
                }

                // Decrypt if a key is specified.
                if (Flags.HasFlag(StegoMessageFlags.Encrypted) && _cryptoProvider != null)
                {
                    try
                    {
                        encodedData = _cryptoProvider.Decrypt(encodedData);
                    }
                    catch (CryptographicException ex)
                    {
                        throw new StegoCryptoException("Could not decrypt data.", ex);
                    }
                }

                // Decompress the array.
                if (Flags.HasFlag(StegoMessageFlags.Compressed))
                {
                    encodedData = Compression.Decompress(encodedData);
                }

                // Decode array.
                if (Flags.HasFlag(StegoMessageFlags.Encoded))
                {
                    Decode(encodedData);
                }
            }
        }

        /// <summary>
        /// This method takes a byte array and retrieves the contents and adds it to the StegoMessage.
        /// </summary>
        private void Decode(byte[] _byteArray)
        {
            using (MemoryStream tempStream = new MemoryStream(_byteArray))
            {
                // Read input files.
                int numberOfFiles = tempStream.ReadInt();
                for (int i = 0; i < numberOfFiles; i++)
                {
                    InputFiles.Add(tempStream.ReadInputFile());
                }

                // Read text message.
                TextMessage = tempStream.ReadString();
            }
        }

        /// <summary>
        /// This method returns a byte array representing the data within the StegoMessage.
        /// </summary>
        private byte[] Encode()
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                // Write input files.
                tempStream.Write(InputFiles.Count);
                foreach (InputFile inputFile in InputFiles)
                {
                    tempStream.Write(inputFile);
                }

                // Write text message.
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
            // Encode and compress array.
            byte[] encodedData = Encode();
            SetFlag(StegoMessageFlags.Encoded, true);

            // Compress data.
            byte[] compressedData = Compression.Compress(encodedData);
            if (compressedData.Length < encodedData.Length)
            {
                encodedData = compressedData;
                SetFlag(StegoMessageFlags.Compressed, true);
            }
            else
            {
                SetFlag(StegoMessageFlags.Compressed, false);
            }

            // Encrypt if key is specified.
            if (_cryptoProvider != null && _cryptoProvider.Key != null)
            {
                encodedData = _cryptoProvider.Encrypt(encodedData);
                SetFlag(StegoMessageFlags.Encrypted, true);
            }
            else
            {
                SetFlag(StegoMessageFlags.Encrypted, false);
            }

            // Combine all the data using a MemoryStream.
            using (MemoryStream tempStream = new MemoryStream())
            {
                // Do not write at the beginning of the stream.
                // Allocate some space for the int that contains size.
                // Also allocate space for a byte which contains our flags.
                tempStream.Seek(sizeof(int) + sizeof(byte), SeekOrigin.Begin);

                // Write encoded data
                tempStream.Write(encodedData, true);

                // Write hash of encoded data
                tempStream.Write(encodedData.ComputeSHAHash(), true);

                // Sign if private key is specified
                if (!string.IsNullOrEmpty(PrivateSigningKey))
                {
                    // Create RSA provider and set key
                    RSAProvider rsa = new RSAProvider();
                    rsa.SetKey(PrivateSigningKey);

                    // Sign and write data
                    tempStream.Write(rsa.SignData(encodedData), true);
                    Console.WriteLine("Message has been signed");

                    // Set signed flag
                    SetFlag(StegoMessageFlags.Signed, true);

                    // Reset key
                    PrivateSigningKey = null;
                }

                // Go back to beginning of stream and write length and flags.
                tempStream.Seek(0, SeekOrigin.Begin);
                tempStream.Write((int)tempStream.Length - sizeof(int));
                tempStream.WriteByte((byte) Flags);

                return tempStream.ToArray();
            }
        }

        /// <summary>
        /// This method is used to set a flag to indicate what operations has been done to the StegoMessage.
        /// </summary>
        private void SetFlag(StegoMessageFlags _flag, bool _state)
        {
            // Check if current flag state does not equal wanted state.
            if (_state != Flags.HasFlag(_flag))
            {
                // Flip state.
                Flags ^= _flag;
            }
        }

        /// <summary>
        /// This method returns the compressed size of the data stored in this StegoMessage.
        /// </summary>
        public long GetCompressedSize()
        {
            return ToByteArray().Length;
        }
    }
}
