using Stegosaurus.Exceptions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Stegosaurus.Carrier
{
    public class ImageCarrier : ICarrierMedia
    {
        public byte[] ByteArray { get; set; }

        public int SamplesPerVertex => 3;

        private readonly Bitmap innerImage;

        public Image InnerImage => innerImage;

        /// <summary>
        /// Main constructer. Gets image, checks if null pointer and sets private variable to cloned image if not null.
        /// </summary>
        public ImageCarrier(Image _innerImage)
        {
            if (_innerImage == null)
                throw new ArgumentNullException("Invalid input image in ImageCarrier.\n");

            // Clone image or convert to PNG
            if (Equals(_innerImage.RawFormat, ImageFormat.Png))
            {
                innerImage = (Bitmap) _innerImage.Clone();
            }
            else
            {
                innerImage = new Bitmap( _innerImage.Width, _innerImage.Height, PixelFormat.Format24bppRgb);
                using (Graphics graphics = Graphics.FromImage(innerImage))
                {
                    graphics.DrawImage(_innerImage, new Rectangle(0, 0, _innerImage.Width, _innerImage.Height));
                }
            }
            _innerImage.Dispose();

            Decode();
        }

        /// <summary>
        /// Gets file path to image file and sends image to constructer.
        /// </summary>
        /// <param name="_filePath"></param>
        public ImageCarrier(string _filePath) : this(LoadImageFromFile(_filePath))
        {
        }

        /// <summary>
        /// Locks innerImage in system memory and returns instance of BitmapData
        /// </summary>
        /// <returns></returns>
        private BitmapData LockBitmap()
        {
            Rectangle imageRectangle = new Rectangle(new Point(0, 0), innerImage.Size);
            return innerImage.LockBits(imageRectangle, ImageLockMode.ReadWrite, innerImage.PixelFormat);
        }

        /// <summary>
        /// Custom Image.FromFile method, since that method does not release the file handle
        /// </summary>
        private static Image LoadImageFromFile(string _filePath)
        {
            try
            {
                return Image.FromStream(new MemoryStream(File.ReadAllBytes(_filePath)));
            }
            catch (ArgumentException)
            {
                throw new InvalidImageFileException("Could not read image from stream.", _filePath);
            }
        }

        /// <summary>
        /// Moves data from innerImage into ByteArray
        /// </summary>
        public void Decode()
        {
            // Lock bits
            BitmapData imageData = LockBitmap();

            // Calculate the bytelength of the pixels and allocate memory for it
            int imageDataLength = Math.Abs(imageData.Height * imageData.Stride);
            ByteArray = new byte[imageDataLength];

            // Copy the pixel array from the innerImage to ByteArray
            Marshal.Copy(imageData.Scan0, ByteArray, 0, imageDataLength);

            // Unlock bits
            innerImage.UnlockBits(imageData);
        }

        /// <summary>
        /// Moves data from ByteArray into innerImage.
        /// </summary>
        public void Encode()
        {
            // Lock bits
            BitmapData imageData = LockBitmap();

            // Copy the pixel array from ByteArray to the innerImage
            Marshal.Copy(ByteArray, 0, imageData.Scan0, ByteArray.Length);

            // Unlock bits
            innerImage.UnlockBits(imageData);
        }

        /// <summary>
        /// Encodes and saves file to destination.
        /// </summary>
        /// <param name="destination"></param>
        public void SaveToFile(string destination)
        {
            Encode();
            innerImage.Save(destination, ImageFormat.Png);
        }
    }
}
