namespace Stegosaurus.Carrier
{
    abstract class CarrierMedia
    {
        public byte[] InnerArray { get; protected set; }

        public abstract void Encode();
        public abstract void Decode();

        public virtual void SaveToFile(string destination)
        {
            Encode();
            /* Do stuff.. */
        }
    }
}
