using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm
{
    interface IStegoAlgorithm
    {
        CarrierMedia Carrier { get; set; }

        long ComputeBandwidth();

        void Embed(StegoMessage message);

        StegoMessage Extract();
    }
}
