using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Cryptography
{
    public static class RC4
    {
        /// <summary>
        /// http://stackoverflow.com/questions/7217627/is-there-anything-wrong-with-this-rc4-encryption-code-in-c-sharp
        /// </summary>
        /// <param name="_data">Data to encrypt</param>
        /// <returns>Returns the cypher(the encrypted byte array)</returns>
        public static byte[] Encrypt(byte[] _data, byte[] _key)
        {
            int a, i, j, k, tmp, maxKeySize = 256;
            int[] key, box;
            byte[] cipher;

            key = new int[maxKeySize];
            box = new int[maxKeySize];
            cipher = new byte[_data.Length];

            // Copies EncrytionKey, into int[] Key, byte by byte, and repeats the process up to maxKeySize,  to ensure the same key lenght.
            for (i = 0; i < maxKeySize; i++)
            {
                key[i] = _key[i % _key.Length];
                box[i] = i;
            }
            // Swaps data elements in int[] box, by exchanging int[i] with int[j].
            for (j = i = 0; i < maxKeySize; i++)
            {
                j = ( j + box[i] + key[i] ) % maxKeySize;
                tmp = box[i];
                box[i] = box[j];
                box[j] = tmp;
            }
            // Further swaps elements in int[] box, and assigns them to byte[] cipher which is returned to method call.
            for (a = j = i = 0; i < _data.Length; i++)
            {
                a++;
                a %= maxKeySize;
                j += box[a];
                j %= maxKeySize;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                k = box[( ( box[a] + box[j] ) % maxKeySize )];
                cipher[i] = (byte) ( _data[i] ^ k );
            }
            return cipher;
        }

        public static byte[] Decrypt(byte[] _data, byte[] _key)
        {
            return Encrypt(_data, _key);
        }
    }
}
