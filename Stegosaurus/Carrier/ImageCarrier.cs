using Stegosaurus.Exceptions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Stegosaurus.Carrier
{
    public class ImageCarrier : ICarrierMedia
    {
        private readonly Bitmap image;

        public byte[] ByteArray { get; set; }

        public int BytesPerSample => 3;

        /// <summary>
        /// Returns the inner instance of Image.
        /// </summary>
        public Image InnerImage => image;

        /// <summary>
        /// Construct ImageCarrier from an instance of Image.
        /// </summary>
        public ImageCarrier(Bitmap _image)
        {
            if (_image == null)
            {
                throw new StegosaurusException("Image can not be null.");
            }

            // Use original image if it meets standards, otherwise convert it to 24 bpp
            if (Equals(_image.RawFormat, ImageFormat.Png) && _image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                image = _image;
            }
            else
            {
                // Copy image
                Bitmap newImage = new Bitmap(_image.Width, _image.Height, PixelFormat.Format24bppRgb);
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));
                }

                image = newImage;
            }

            Decode();
        }

        /// <summary>
        /// Gets file path to image file and passes instace of Image to constructor.
        /// </summary>
        /// <param name="_filePath"></param>
        public ImageCarrier(string _filePath) : this(LoadImageFromFile(_filePath))
        {
        }

        /// <summary>
        /// Locks innerImage in system memory and returns instance of BitmapData.
        /// </summary>
        private BitmapData LockBitmap()
        {
            return image.LockBits(new Rectangle(new Point(0, 0), image.Size), ImageLockMode.ReadWrite, image.PixelFormat);
        }

        /// <summary>
        /// Load Image from a specified file.
        /// Alternative to Image.FromFile, which does not always release file handle.
        /// </summary>
        private static Bitmap LoadImageFromFile(string _filePath)
        {
            try
            {
                return (Bitmap) Image.FromStream(new MemoryStream(File.ReadAllBytes(_filePath)));
            }
            catch (ArgumentException)
            {
                throw new InvalidImageFileException("Could not read image from stream.", _filePath);
            }
        }

        public void Decode()
        {
            // Lock bits
            // BitmapData imageData = LockBitmap();

            // Calculate the bytelength of the pixels and allocate memory for it
            //int imageDataLength = Image.GetPixelFormatSize(image.PixelFormat) / 8 * image.Width * image.Height;
            //ByteArray = new byte[imageDataLength];

            int pos = 0;
            ByteArray = new byte[image.Width * image.Height * BytesPerSample];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    ByteArray[pos++] = c.R;
                    ByteArray[pos++] = c.G;
                    ByteArray[pos++] = c.B;
                }
            }

            // Copy the pixel array from the innerImage to ByteArray
            // Marshal.Copy(imageData.Scan0, ByteArray, 0, imageDataLength);

            // Unlock bits
            // image.UnlockBits(imageData);
        }

        /// <summary>
        /// Moves data from ByteArray into innerImage.
        /// </summary>
        public void Encode()
        {
            // Lock bits
            // BitmapData imageData = LockBitmap();

            // Copy the pixel array from ByteArray to the innerImage
            // Marshal.Copy(ByteArray, 0, imageData.Scan0, ByteArray.Length);
            int pos = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    image.SetPixel(i, j, Color.FromArgb(ByteArray[pos++], ByteArray[pos++], ByteArray[pos++]));
                }
            }

            // Unlock bits
            // image.UnlockBits(imageData);
        }

        /// <summary>
        /// Encodes and saves file to destination.
        /// </summary>
        public void SaveToFile(string _destination)
        {
            var old = ByteArray;
            Encode();
            image.Save(_destination, ImageFormat.Png);

            // todo remove this when sure that bug is gone
            ImageCarrier car = new ImageCarrier(_destination);
            if (!old.SequenceEqual(car.ByteArray))
            {
                throw new StegosaurusException("ImageCarrier is possibly bugged.");
            }
        }
    }
}
