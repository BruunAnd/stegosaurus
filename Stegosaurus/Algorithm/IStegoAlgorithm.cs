using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm
{
    interface IStegoAlgorithm
    {
        ICarrierMedia CarrierMedia { get; set; }

        long ComputeBandwidth();

        void Embed(StegoMessage message);

        StegoMessage Extract();
    }
}
