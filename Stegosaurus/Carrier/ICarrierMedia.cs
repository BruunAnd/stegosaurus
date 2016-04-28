namespace Stegosaurus.Carrier
{
    public interface ICarrierMedia
    {
        byte[] ByteArray { get; set; }
        int SamplesPerVertex { get; }

        void Encode();
        void Decode();
        void SaveToFile(string destination);
    }
}
