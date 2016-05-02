using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Sample
    {
        public byte[] Bytes { get; private set; }
        public int Value { get; set; }
        public int TargetValue { get; set; }
        
        public Sample(byte[] _bytes)
        {
            Bytes = _bytes;
        }
    }
}
