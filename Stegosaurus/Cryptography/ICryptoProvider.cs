namespace Stegosaurus.Cryptography
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// The key to be used, either in encryption or decryption.
        /// </summary>
        byte[] Key { get; set; }

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

        /// <summary>
        /// Set the Key from a string
        /// </summary>
        void SetKey(string _keyString);

        /// <summary>
        /// Generates and returns a key which can be used with the algorithm
        /// </summary>
        byte[] GenerateKey();

        /// <summary>
        /// Encrypts and returns encrypted data
        /// </summary
        byte[] Encrypt(byte[] _data);

        /// <summary>
        /// Decrypts and returns decrypted data
        /// </summary>
        byte[] Decrypt(byte[] _data);
    }
}
