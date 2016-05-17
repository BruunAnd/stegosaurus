namespace Stegosaurus.Cryptography
{
    public class SavedPublicKey
    {
        /// <summary>
        /// The Alias of the owner of this public key.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// The public Key used to verify data.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Constructs a new SavedPublicKey.
        /// </summary>
        public SavedPublicKey(string _alias, string _key)
        {
            Alias = _alias;
            Key = _key;
        }
    }
}
