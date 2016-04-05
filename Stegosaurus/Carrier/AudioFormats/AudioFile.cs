using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stegosaurus.Carrier.AudioFormats
{
    abstract class AudioFile
    {
        public short NumberOfChannels { get; private set; }
        public int SampleRate { get; private set; }
        public int ByteRate { get; private set; }
        public short BlockAlign { get; private set; }
        public short BitsPerSample { get; private set; }

        private byte[] innerData;

        public AudioFile(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                throw new ArgumentException("Input file does not exist.");
            }

            Parse();
        }

        /// <summary>
        /// Parses an audio file by reading its headers and samples
        /// </summary>
        public abstract void Parse();

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
