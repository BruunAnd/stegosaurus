using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Stegosaurus.Extensions;

namespace Stegosaurus.Carrier.AudioFormats
{
    class WaveFile : AudioFile
    {
        // Constants
        private static readonly byte[] RiffHeader = {82, 73, 70, 70};
        private static readonly byte[] FormatHeader = { 87, 65, 86, 69, 102, 109, 116, 32 };
        private static readonly byte[] DataHeader = { 100, 97, 116, 97 };

        // Wave-specific properties
        public int ChunkSize { get; set; }
        public int FormatSubChunkSize { get; set; }
        public int DataSubChunkSize { get; set; }
        public short AudioFormat { get; set; }

        public WaveFile(string _filePath) : base(_filePath) { }

        public override void Parse(string _filePath)
        {
            using (FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                // Check if RIFF header is corret
                if (!fileStream.ReadBytes(RiffHeader.Length).SequenceEqual(RiffHeader))
                {
                    throw new Exception("File does not contain RIFF header");
                }

                // Read chunk size
                ChunkSize = fileStream.ReadInt();

                // Checks if format header is correct
                if (!fileStream.ReadBytes(FormatHeader.Length).SequenceEqual(FormatHeader))
                {
                    throw new Exception("File does not contain format header");
                }

                // Read sub chunk size
                FormatSubChunkSize = fileStream.ReadInt();

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

                // If format block is larger than 16 bytes, read FormatSubChunkSize - 16 bytes
                if (FormatSubChunkSize > 16)
                {
                    fileStream.ReadBytes(FormatSubChunkSize - 16);
                }

                // Checks if data header is correct
                if (!fileStream.ReadBytes(DataHeader.Length).SequenceEqual(DataHeader))
                {
                    throw new Exception("File does not contain data header");
                }

                // Reads data sub chunk size
                DataSubChunkSize = fileStream.ReadInt();

                // Read samples
                innerData = fileStream.ReadBytes(DataSubChunkSize);
            }
        }

        public override byte[] ToArray()
        {
            throw new NotImplementedException();
        }

        

    }
}
