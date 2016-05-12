using Stegosaurus.Carrier.AudioFormats;
using System;
using System.IO;

namespace Stegosaurus.Carrier
{
    class AudioCarrier : ICarrierMedia
    {
        public byte[] ByteArray { get; set; }

        public int BytesPerSample => audioFile.BitsPerSample / 8;

        private readonly AudioFile audioFile;

        public AudioCarrier(string _filePath)
        {
            // Check if format is supported
            if (_filePath.EndsWith(".wav"))
                audioFile = new WaveFile(_filePath);
            else
                throw new ArgumentException("Input file format is not supported.");

            Decode();        
        }

        public void Decode()
        {
            ByteArray = audioFile.CopyInnerData();
        }

        public void Encode()
        {
            audioFile.SetInnerData(ByteArray);
        }

        public void SaveToFile(string _destination)
        {
            Encode();
            File.WriteAllBytes(_destination, audioFile.ToArray());
        }
    }
}
