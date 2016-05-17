using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stegosaurus.Cryptography
{
    public class PublicKeyList
    {
        private static List<SavedPublicKey> publicKeyList = new List<SavedPublicKey>();

        /// <summary>
        /// Adds a public key to the keystore.
        /// </summary>
        public static void Add(string _alias, string _publicKeyLocation)
        {
            // todo check if file exists
            SavedPublicKey key = new SavedPublicKey(_alias, File.ReadAllText(_publicKeyLocation));
            publicKeyList.Add(key);
        }

        /// <summary>
        /// Returns the list of public keys.
        /// </summary>
        public static List<SavedPublicKey> GetKeyList()
        {
            return publicKeyList;
        }
    }
}
