using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm
{
    public interface IStegoAlgorithm
    {
        /// <summary>
        /// Returns the name of the algorithm
        /// </summary>
        string Name { get; }

        // Returns the CarrierMedia used by this instance of the algorithm
        ICarrierMedia CarrierMedia { get; }

        /// <summary>
        /// Returns the data capacity of the carrier media with the given StegoAlgorithm
        /// </summary>
        long ComputeBandwidth(ICarrierMedia CarrierMedia);

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
