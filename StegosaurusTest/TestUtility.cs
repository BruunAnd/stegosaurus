using System;

namespace StegosaurusTest
{
    public static class TestUtility
    {
        private static readonly Random rand = new Random();

        public static byte[] GetRandomBytes(int _size)
        {
            byte[] retArray = new byte[_size];
            rand.NextBytes(retArray);
            return retArray;    
        }
    }
}
