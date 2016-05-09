using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm.CommonSample
{
    public class Sample : IEquatable<Sample>
    {
        public byte[] Values;

        public int ModValue { get; }

        public int TargetValue { get; set; }

        public Sample(params byte[] _values)
        {
            Values = _values;
            ModValue = Values.Sum(b => b) % 2;        
        }

        public void ForceChanges()
        {
            Values[0] ^= 0x1;
        }

        public int DistanceTo(Sample otherSample)
        {
            int distance = 0;

            for (int i = 0; i < otherSample.Values.Length; i++)
            {
                distance += (int) Math.Pow(Values[i] - otherSample.Values[i], 2);
            }

            return distance;
        }

        public override int GetHashCode()
        {
            return Values.ComputeHash();
        }

        public override string ToString()
        {
            string retString = string.Empty;
            foreach (byte val in Values)
                retString += " " + val;
            return retString;
        }

        public bool Equals(Sample other)
        {
            return Values.SequenceEqual(other.Values);
        }
    }
}
