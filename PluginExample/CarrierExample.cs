using System;
using System.Drawing;
using Stegosaurus.Carrier;

namespace PluginExample
{
    public class CarrierExample : ICarrierMedia
    {
        public byte[] ByteArray { get; set; }
        public string OutputExtension => null;
        public Image Thumbnail => null;
        public int BytesPerSample => 0;

        public bool IsExtensionCompatible(string _extension)
        {
            return false;
        }

        public void LoadFromFile(string _filePath)
        {
            throw new NotImplementedException();
        }

        public void Encode()
        {
            throw new NotImplementedException();
        }

        public void Decode()
        {
            throw new NotImplementedException();
        }

        public void SaveToFile(string _destination)
        {
            throw new NotImplementedException();
        }
    }
}
