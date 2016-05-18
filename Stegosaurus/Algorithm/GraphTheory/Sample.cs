using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Sample
    {
        public byte[] Values;
        public byte ModValue;
        public byte TargetValue;
        
        public Sample(byte[] _values)
        {
            Values = (byte[])_values.Clone();
        }
    }
}
