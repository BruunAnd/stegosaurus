using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm
{
    /* Example of an algorithm implementation */

    class LsbAlgorithm : IStegoAlgorithm
    {
        public CarrierMedia Carrier { get; set; }

        /// <summary>
        /// Creates an instance of the Least Significant Bit algorithm.
        /// </summary>
        /// <param name="carrier">CarrierMedia to perform stego-actions on.</param>
        public LsbAlgorithm(CarrierMedia carrier)
        {
            Carrier = carrier;
        }

        public long ComputeBandwidth()
        {
            return 0;
        }

        public void Embed(StegoMessage message)
        {
            /* Do stuff with Carrier */
            /* We have access to Carrier.InnerArray */
        }

        public StegoMessage Extract()
        {
            /* Read stuff from Carrier and handle it */
            return null;
        }

    }
}
