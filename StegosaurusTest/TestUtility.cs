using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegosaurusTest
{
    public static class TestUtility
    {
        private static Random _rand = new Random();

        public static byte[] GetRandomBytes(int _size)
        {
            byte[] retArray = new byte[_size];
            _rand.NextBytes(retArray);
            return retArray;    
        }
    }
}
