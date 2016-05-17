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
        private readonly Bitmap image;

        public byte[] ByteArray { get; set; }

        public int BytesPerSample => 3;

        /// <summary>
        /// Returns the inner instance of Image.
        /// </summary>
        public Image Image => image;

        /// <summary>
        /// Construct ImageCarrier from an instance of Image.
        /// </summary>
        public ImageCarrier(Image _image)
        {
            if (_image == null)
            {
                throw new StegosaurusException("Image can not be null.");
            }

            // Clone image or convert to PNG
            if (_image.RawFormat.Equals(ImageFormat.Png) && _image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                image = (Bitmap) _image.Clone();
            }
            else
            {
                image = new Bitmap(_image.Width, _image.Height, PixelFormat.Format24bppRgb);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height));
                }
            }
            _image.Dispose();

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
            Rectangle imageRectangle = new Rectangle(new Point(0, 0), image.Size);
            return image.LockBits(imageRectangle, ImageLockMode.ReadWrite, image.PixelFormat);
        }

        /// <summary>
        /// Load Image from a specified file.
        /// Alternative to Image.FromFile, which does not always release file handle.
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
            image.UnlockBits(imageData);
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
            image.UnlockBits(imageData);
        }

        /// <summary>
        /// Encodes and saves file to destination.
        /// </summary>
        public void SaveToFile(string _destination)
        {
            Encode();
            image.Save(_destination, ImageFormat.Png);
        }
    }
}
