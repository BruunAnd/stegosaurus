using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Sample
    {
        public byte[] Bytes;
        public short Value;
        public short TargetValue;
        
        public Sample(byte[] _bytes)
        {
            Bytes = (byte[])_bytes.Clone();
        }
    }
}
