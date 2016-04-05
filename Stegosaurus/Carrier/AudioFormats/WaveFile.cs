using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Stegosaurus.Carrier.AudioFormats
{
    class WaveFile : AudioFile
    {
        // Wave-specific properties
        public int ChunkSize { get; set; }
        public int FormatSubChunkSize { get; set; }
        public int DataSubChunkSize { get; set; }

        public WaveFile(string _filePath) : base(_filePath) { }

        public override void Parse()
        {
            throw new NotImplementedException();
        }

        public override byte[] ToArray()
        {
            throw new NotImplementedException();
        }


    }
}
