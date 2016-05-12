namespace Stegosaurus.Carrier
{
    public interface ICarrierMedia
    {
        byte[] ByteArray { get; set; }
        int BytesPerSample { get; }

        void Encode();
        void Decode();
        void SaveToFile(string _destination);
    }
}
