namespace Stegosaurus.Carrier
{
    interface ICarrierMedia
    {
        byte[] Bitstream { get; set; }

        void Encode();
        void Decode();
        void SaveToFile(string destination);
    }
}
