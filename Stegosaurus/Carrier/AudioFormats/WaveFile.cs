using System.IO;
using System.Linq;
using Stegosaurus.Utility.Extensions;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Carrier.AudioFormats
{
    public class WaveFile : AudioFile
    {
        // Constants
        private static readonly byte[] RiffHeader = {82, 73, 70, 70};
        private static readonly byte[] FormatHeader = { 87, 65, 86, 69, 102, 109, 116, 32 };
        private static readonly byte[] DataHeader = { 100, 97, 116, 97 };

        /// <summary>
        /// Get or set the chunk size.
        /// </summary>
        public int ChunkSize { get; private set; }

        /// <summary>
        /// Get or set the size of the format subchunk.
        /// </summary>
        public int FormatSubChunkSize { get; private set; }

        /// <summary>
        /// Get or set the size of the data subchunk.
        /// </summary>
        public int DataSubChunkSize { get; private set; }

        /// <summary>
        /// Get or set the audio format.
        /// </summary>
        public short AudioFormat { get; private set; }

        /// <summary>
        /// Construct a WaveFile from a file path.
        /// </summary>
        public WaveFile(string _filePath) : base(_filePath)
        {
        }

        public override void Parse(string _filePath)
        {
            using (FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                // Check if RIFF header is correct
                if (!fileStream.ReadBytes(RiffHeader.Length).SequenceEqual(RiffHeader))
                {
                    throw new InvalidWaveFileException("Could not read RIFF header.", _filePath);
                }

                // Read chunk size
                ChunkSize = fileStream.ReadInt();

                // Checks if format header is correct
                if (!fileStream.ReadBytes(FormatHeader.Length).SequenceEqual(FormatHeader))
                {
                    throw new InvalidWaveFileException("File does not contain format header.", _filePath);
                }

                // Read sub chunk size
                FormatSubChunkSize = fileStream.ReadInt();
                if (FormatSubChunkSize > 16)
                {
                    throw new InvalidWaveFileException("Non-PCM files are not supported.", _filePath);
                }

                // Read audio format
                AudioFormat = fileStream.ReadShort();

                // Read number of channels
                NumberOfChannels = fileStream.ReadShort();

                // Read number of samples per second
                SampleRate = fileStream.ReadInt();

                // Read bytes processed per second
                ByteRate = fileStream.ReadInt();

                // Read block align
                BlockAlign = fileStream.ReadShort();

                // Read bits per sample
                BitsPerSample = fileStream.ReadShort();

                // Checks if data header is correct
                if (!fileStream.ReadBytes(DataHeader.Length).SequenceEqual(DataHeader))
                {
                    throw new InvalidWaveFileException("File does not contain data header.", _filePath);
                }

                // Reads data sub chunk size
                DataSubChunkSize = fileStream.ReadInt();

                // Read samples
                InnerData = fileStream.ReadBytes(DataSubChunkSize);
            }
        }

        public override byte[] ToArray()
        {
            using (MemoryStream tempStream = new MemoryStream())
            {
                // Add header data
                tempStream.Write(RiffHeader);
                tempStream.Write(ChunkSize);
                tempStream.Write(FormatHeader);
                tempStream.Write(FormatSubChunkSize);
                tempStream.Write(AudioFormat);
                tempStream.Write(NumberOfChannels);
                tempStream.Write(SampleRate);
                tempStream.Write(ByteRate);
                tempStream.Write(BlockAlign);
                tempStream.Write(BitsPerSample);

                // Write data
                tempStream.Write(DataHeader);
                tempStream.Write(DataSubChunkSize);
                tempStream.Write(InnerData);

                return tempStream.ToArray();
            }
        }
    }
}
