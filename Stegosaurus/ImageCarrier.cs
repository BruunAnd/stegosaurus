using System.Drawing;

namespace Stegosaurus
{
    class ImageCarrier : CarrierMedia
    {

        private Image _innerImage;

        public ImageCarrier(string sourceFile)
        {
            _innerImage = (Image) Image.FromFile(sourceFile).Clone();
        }

        public override void Decode()
        {
            /* Get inner data from image... */
            _innerArray = null;
        }

        public override void Encode()
        {
            /* Write inner data to image... */
        }

    }
}
