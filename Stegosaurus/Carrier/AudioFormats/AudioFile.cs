using System;
using System.IO;

namespace Stegosaurus.Carrier.AudioFormats
{
    public abstract class AudioFile
    {
        /// <summary>
        /// Get or set the number of channels.
        /// </summary>
        public short NumberOfChannels { get; protected set; }

        /// <summary>
        /// Get or set the sample rate.
        /// </summary>
        public int SampleRate { get; protected set; }

        /// <summary>
        /// Get or set the byte rate.
        /// </summary>
        public int ByteRate { get; protected set; }

        /// <summary>
        /// Get or set the block align.
        /// </summary>
        public short BlockAlign { get; protected set; }

        /// <summary>
        /// Get or set the bits per sample.
        /// </summary>
        public short BitsPerSample { get; protected set; }

        protected byte[] InnerData;

        /// <summary>
        /// Construct an AudioFile from a file path.
        /// </summary>
        protected AudioFile(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                throw new ArgumentException("Input file does not exist.");
            }

            Parse(_filePath);
        }

        /// <summary>
        /// Parses an audio file by reading its headers and samples.
        /// </summary>
        public abstract void Parse(string _filePath);

        /// <summary>
        /// Reconstructs and returns the entire byte array of the file, including headers.
        /// </summary>
        public abstract byte[] ToArray();

        /// <summary>
        /// Sets the innerData of the audio file, which contains the original samples.
        /// </summary>
        public virtual void SetInnerData(byte[] _innerData)
        {
            InnerData = _innerData;
        }

        /// <summary>
        /// Returns a clone of the innerData array which can be manipulated by an algorithm.
        /// </summary>
        public virtual byte[] CopyInnerData()
        {
            return (byte[]) InnerData.Clone();
        }
    }
}
