using Stegosaurus.Carrier;

namespace StegosaurusTest
{
    internal class ImageCarrier : ICarrierMedia
    {
        private string coverFile;

        public ImageCarrier(string coverFile)
        {
            this.coverFile = coverFile;
        }
    }
}