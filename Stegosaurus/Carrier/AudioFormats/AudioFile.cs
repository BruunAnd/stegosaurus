using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stegosaurus.Carrier.AudioFormats
{
    abstract class AudioFile
    {
        public short NumberOfChannels { get; protected set; }
        public int SampleRate { get; protected set; }
        public int ByteRate { get; protected set; }
        public short BlockAlign { get; protected set; }
        public short BitsPerSample { get; protected set; }

        protected byte[] innerData;

        public AudioFile(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                throw new ArgumentException("Input file does not exist.");
            }

            Parse(_filePath);
        }

        /// <summary>
        /// Parses an audio file by reading its headers and samples
        /// </summary>
        public abstract void Parse(string _filePath);

        /// <summary>
        /// Reconstructs and returns the entire byte array of the file, including headers
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToArray();

        /// <summary>
        /// Sets the innerData of the audio file, which contains the original samples
        /// </summary>
        public virtual void SetInnerData(byte[] _innerData)
        {
            innerData = _innerData;
        }

        /// <summary>
        /// Returns a clone of the innerData array which can be manipulated by an algorithm
        /// </summary>
        public virtual byte[] CopyInnerData()
        {
            return (byte[]) innerData.Clone();
        }
    }
}
