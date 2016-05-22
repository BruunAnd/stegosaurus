using System.Drawing;

namespace Stegosaurus.Carrier
{
    public interface ICarrierMedia
    {
        /// <summary>
        /// The byte array containing samples of the carrier media.
        /// This array is to be used by algorithms.
        /// </summary>
        byte[] ByteArray { get; set; }

        /// <summary>
        /// The default output extension for this carrier media.
        /// Example: 
        /// </summary>
        string OutputExtension { get; }

        /// <summary>
        /// Get the thumbnail associated with this carrier media.
        /// </summary>
        Image Thumbnail { get; }

        /// <summary>
        /// The amount of bytes per sample, where a sample is defined as a sequence of bytes.
        /// For example, the pixels in an image are samples, where the amount of bytes is the amount of channels.
        /// </summary>
        int BytesPerSample { get; }

        /// <summary>
        /// Check if a certain extension is compatible with this carrier media.
        /// </summary>
        bool IsExtensionCompatible(string _extension);

        /// <summary>
        /// Open file from specified path.
        /// </summary>
        void LoadFromFile(string _filePath);

        /// <summary>
        /// Encodes ByteArray back into the carrier media.
        /// </summary>
        void Encode();

        /// <summary>
        /// Decodes the carrier media and sets ByteArray to the inner data.
        /// </summary>
        void Decode();

        /// <summary>
        /// Saves the carrier media to the specified destination.
        /// </summary>
        void SaveToFile(string _destination);
    }
}
