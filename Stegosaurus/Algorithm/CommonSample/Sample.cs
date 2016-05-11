using System;
using System.Linq;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm.CommonSample
{
    public class Sample : IEquatable<Sample>, ICloneable
    {
        private byte[] backingValuesField;

        public byte[] Values
        {
            get
            {
                return backingValuesField;
            }
            set
            {
                backingValuesField = value;
                UpdateModValue();
            }
        }

        public int LastDistance { get; set; }

        public int ModValue { get; set; }

        public Sample(params byte[] _values)
        {
            Values = _values;
        }

        public void ForceChanges()
        {
            Values[Values.Length - 1] ^= 0x1;
            UpdateModValue();
        }

        public void UpdateModValue()
        {
            ModValue = Values.Sum(val => val) % 2;
        }

        public int DistanceTo(Sample otherSample)
        {
            int distance = 0;

            for (int i = 0; i < otherSample.Values.Length; i++)
            {
                distance += (int) Math.Pow(Values[i] - otherSample.Values[i], 2);
            }

            LastDistance = distance;

            return distance;
        }

        public override int GetHashCode()
        {
            return Values.ComputeHash();
        }

        public bool Equals(Sample other)
        {
            return Values.SequenceEqual(other.Values);
        }

        public object Clone()
        {
            return new Sample((byte[]) Values.Clone());
        }
    }
}
