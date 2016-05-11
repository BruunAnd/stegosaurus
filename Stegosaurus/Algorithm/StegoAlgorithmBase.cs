using System;
using System.ComponentModel;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using System.Threading;

namespace Stegosaurus.Algorithm
{
    public abstract class StegoAlgorithmBase
    {
        /// <summary>
        /// Get the name of the algorithm
        /// </summary>
        [Browsable(false)]
        public abstract string Name { get; }

        /// <summary>
        /// Get or set crypto provider
        /// </summary>
        [Browsable(false)]
        public virtual ICryptoProvider CryptoProvider { get; set; }

        /// <summary>
        /// Get or set CarrierMedia
        /// </summary>
        [Browsable(false)]
        public virtual ICarrierMedia CarrierMedia { get; set; }

        /// <summary>
        /// Get Seed used in pseudo-random pattern
        /// </summary>
        [Browsable(false)]
        protected virtual int Seed => CryptoProvider?.Seed ?? 0;

        /// <summary>
        /// Returns the data capacity of the carrier media with the given StegoAlgorithm
        /// </summary>
        public abstract long ComputeBandwidth();

        /// <summary>
        /// Embeds a StegoMessage in the public ByteArray of the CarrierMedia
        /// </summary>
        public abstract void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct);

        /// <summary>
        /// Returns a StegoMessage by extracting from the public ByteArray of the CarrierMedia
        /// </summary>
        public abstract StegoMessage Extract();
    }
}
