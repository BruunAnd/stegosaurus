using System;
using System.Linq;
using Stegosaurus.Utility.Extensions;
using System.Collections.Generic;
using Stegosaurus.Carrier;

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

        public int DistanceTo(Sample _otherSample)
        {
            int distance = 0;

            for (int i = 0; i < _otherSample.Values.Length; i++)
            {
                distance += (int) Math.Pow(Values[i] - _otherSample.Values[i], 2);
            }

            LastDistance = distance;

            return distance;
        }

        public override int GetHashCode()
        {
            return Values.ComputeHash();
        }

        public bool Equals(Sample _other)
        {
            return Values.SequenceEqual(_other.Values);
        }

        public object Clone()
        {
            return new Sample((byte[]) Values.Clone());
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia.
        /// </summary>
        public static List<Sample> GetSampleListFrom(ICarrierMedia _carrierMedia)
        {
            List<Sample> sampleList = new List<Sample>(_carrierMedia.ByteArray.Length / _carrierMedia.BytesPerSample);

            int currentSample = 0;
            while (currentSample < _carrierMedia.ByteArray.Length)
            {
                byte[] sampleValues = new byte[_carrierMedia.BytesPerSample];

                for (int i = 0; i < _carrierMedia.BytesPerSample; i++)
                {
                    sampleValues[i] = _carrierMedia.ByteArray[currentSample++];
                }

                sampleList.Add(new Sample(sampleValues));
            }

            return sampleList;
        }
    }
}
