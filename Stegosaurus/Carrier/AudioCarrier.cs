using Stegosaurus.Carrier.AudioFormats;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;

namespace Stegosaurus.Carrier
{
    public class AudioCarrier : ICarrierMedia
    {
        public byte[] ByteArray { get; set; }
        public string OutputExtension => ".wav";
        public Image Thumbnail => IconExtractor.ExtractIcon(OutputExtension).ToBitmap();

        public int BytesPerSample => audioFile.BitsPerSample / 8;

        private AudioFile audioFile;

        public bool IsExtensionCompatible(string _extension)
        {
            string[] compatibleExtensions = { ".wav" };
            return compatibleExtensions.Contains(_extension);
        }

        public void LoadFromFile(string _filePath)
        {
            // Check if format is supported
            if (_filePath.EndsWith(".wav"))
            {
                audioFile = new WaveFile(_filePath);
            }
            else
            {
                throw new StegoCarrierException("Audio file is not supported by this carrier.");
            }

            Decode();
        }

        public void Encode()
        {
            audioFile.SetInnerData(ByteArray);
        }

        public void Decode()
        {
            ByteArray = audioFile.CopyInnerData();
        }

        public void SaveToFile(string _destination)
        {
            Encode();
            File.WriteAllBytes(_destination, audioFile.ToArray());
        }
    }
}
