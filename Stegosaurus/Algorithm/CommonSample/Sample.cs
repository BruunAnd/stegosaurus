using System;
using System.Linq;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm.CommonSample
{
    public class Sample : IEquatable<Sample>
    {
        public byte[] Values { get; set; }

        public int ModValue
        {
            get
            {
                return Values.Sum(val => val) % 2;
            }
        }

        public Sample(params byte[] _values)
        {
            Values = _values;
        }

        public void ForceChanges()
        {
            Values[Values.Length - 1] ^= 0x1;
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
            return Values.Aggregate(string.Empty, (current, val) => current + (" " + val));
        }

        public bool Equals(Sample other)
        {
            return Values.SequenceEqual(other.Values);
        }
    }
}
