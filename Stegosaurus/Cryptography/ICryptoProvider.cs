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
        /// The seed to be optionally used by algorithms, typically a hash of the encryption key.
        /// If an implementation of ICryptoProvider is assymetric, it should still provide a symmetric seed.
        /// </summary>
        int Seed { get; }

        byte[] Encrypt(byte[] _data);
        byte[] Decrypt(byte[] _data);
    }
}
