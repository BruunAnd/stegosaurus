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
        private Bitmap image;

        public byte[] ByteArray { get; set; }
        public string OutputExtension => ".png";
        public Image Thumbnail => Image;

        public int BytesPerSample => 3;

        /// <summary>
        /// Returns the inner instance of Image.
        /// </summary>
        public Image Image
        {
            get { return image; }
            set
            {
                // Use original image if it meets standards, otherwise convert it to 24 bpp
                if (Equals(value.RawFormat, ImageFormat.Png) && value.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    image = (Bitmap) value;
                }
                else
                {
                    // Copy image
                    Bitmap newImage = new Bitmap(value.Width, value.Height, PixelFormat.Format24bppRgb);
                    using (Graphics graphics = Graphics.FromImage(newImage))
                    {
                        graphics.DrawImage(value, new Rectangle(0, 0, value.Width, value.Height));
                    }

                    image = newImage;
                }
            }
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

        public unsafe void Decode()
        {
            if (Image == null)
            {
                throw new StegoCarrierException("Image should be set before being decoded.");
            }

            // Lock bits
            BitmapData imageData = LockBitmap();

            // Calculate the bytelength of the pixels and allocate memory for it
            int imageDataLength = imageData.Width * imageData.Height * BytesPerSample;
            ByteArray = new byte[imageDataLength];

            // Get scan0 pointer
            byte* scanPtr = (byte*)imageData.Scan0.ToPointer();

            // Iterate through pixels
            int dstPosition = 0;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    // We have to 'reverse' each pixel as they are in BGR format (we want RGB)
                    int basePosition = j * imageData.Stride + i * BytesPerSample;
                    ByteArray[dstPosition++] = scanPtr[basePosition + 2];
                    ByteArray[dstPosition++] = scanPtr[basePosition + 1];
                    ByteArray[dstPosition++] = scanPtr[basePosition + 0];
                }
            }

            // Unlock bits
            image.UnlockBits(imageData);
        }

        public bool IsExtensionCompatible(string _extension)
        {
            string[] compatibleExtensions = { ".jpg", ".jpeg", ".jpe", ".jfif", ".png", ".gif", ".bmp" };
            return compatibleExtensions.Contains(_extension);
        }

        public void LoadFromFile(string _filePath)
        {
            Image = LoadImageFromFile(_filePath);
            Decode();
        }

        public unsafe void Encode()
        {
            if (Image == null)
            {
                throw new StegoCarrierException("Image should be set before being encoded.");
            }

            // Lock bits
            BitmapData imageData = LockBitmap();

            // Copy the pixel array from ByteArray to the innerImage
            // Marshal.Copy(ByteArray, 0, imageData.Scan0, ByteArray.Length);
            // Get scan0 pointer
            byte* scanPtr = (byte*)imageData.Scan0.ToPointer();

            // Copy the pixel array from ByteArray to the innerImage
            int srcPosition = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // We now have to turn RGB into BGR
                    int basePosition = y * imageData.Stride + x * BytesPerSample;
                    scanPtr[basePosition + 2] = ByteArray[srcPosition++];
                    scanPtr[basePosition + 1] = ByteArray[srcPosition++];
                    scanPtr[basePosition + 0] = ByteArray[srcPosition++];
                }
            }

            // Unlock bits
            image.UnlockBits(imageData);
        }

        public void SaveToFile(string _destination)
        {
            Encode();
            image.Save(_destination, ImageFormat.Png);
        }
    }
}
