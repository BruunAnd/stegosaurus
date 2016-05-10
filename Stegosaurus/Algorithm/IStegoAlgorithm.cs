using System.ComponentModel;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;

namespace Stegosaurus.Algorithm
{
    public interface IStegoAlgorithm
    {
        /// <summary>
        /// Get the name of the algorithm
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get or set crypto provider
        /// </summary>
        ICryptoProvider CryptoProvider { get; set; }

        /// <summary>
        /// Returns the CarrierMedia used by this instance of the algorithm
        /// </summary>
        ICarrierMedia CarrierMedia { get; set; }

        /// <summary>
        /// Returns the data capacity of the carrier media with the given StegoAlgorithm
        /// </summary>
        long ComputeBandwidth();

        /// <summary>
        /// Embeds a StegoMessage in the public ByteArray of the CarrierMedia
        /// </summary>
        void Embed(StegoMessage message);

        /// <summary>
        /// Returns a StegoMessage by extracting from the public ByteArray of the CarrierMedia
        /// </summary>
        StegoMessage Extract();
    }
}
