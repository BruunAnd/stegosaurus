using System.Drawing;

namespace Stegosaurus.Carrier
{
    class ImageCarrier : CarrierMedia
    {

        private Image _innerImage;

        public ImageCarrier(string sourceFile)
        {
            _innerImage = (Image) Image.FromFile(sourceFile).Clone();

            /* Use BitmapData to retrieve data from image
             * Set InnerData to whatever the content is */
        }

        public override void Decode()
        {
            /* Get inner data from image... */
            InnerArray = null;
        }

        public override void Encode()
        {
            /* Write inner data to image... */
            /* InnerImage gets changed */
        }

    }
}
