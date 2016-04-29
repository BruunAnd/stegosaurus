namespace Stegosaurus.Cryptography
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// The key to be used, either in encryption or decryption.
        /// </summary>
        string CryptoKey { get; set; }

        /// <summary>
        /// The name of the algorithm.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The maximum key size in bits.
        /// </summary>
        int KeySize { get; }

        /// <summary>
        /// Some algorithms incorporate a seed to randomize the order in which vertices are exchanged.
        /// </summary>
        int Seed { get; }

        byte[] Encrypt(byte[] _data);
        byte[] Decrypt(byte[] _data);
    }
}
