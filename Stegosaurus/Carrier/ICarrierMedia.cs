namespace Stegosaurus.Carrier
{
    interface ICarrierMedia
    {
        byte[] ByteArray { get; set; }

        void Encode();
        void Decode();
        void SaveToFile(string destination);
    }
}
