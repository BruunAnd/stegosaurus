using System.Collections.Generic;
using System.IO;

namespace Stegosaurus.Cryptography
{
    public static class PublicKeyList
    {
        private static readonly List<SavedPublicKey> InnerList = new List<SavedPublicKey>();

        /// <summary>
        /// Adds a public key to the keystore.
        /// </summary>
        public static void Add(string _alias, string _publicKeyLocation)
        {
            // todo check if file exists
            SavedPublicKey key = new SavedPublicKey(_alias, File.ReadAllText(_publicKeyLocation));
            InnerList.Add(key);
        }

        /// <summary>
        /// Adds an existing SavedPublicKey.
        /// </summary>
        public static void Add(SavedPublicKey _existingKey)
        {
            InnerList.Add(_existingKey);
        }

        /// <summary>
        /// Returns the list of public keys.
        /// </summary>
        public static List<SavedPublicKey> GetKeyList()
        {
            return InnerList;
        }
    }
}
