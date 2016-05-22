using System;
using System.Linq;
using Stegosaurus.Utility.Extensions;
using System.Collections.Generic;
using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm.CommonSample
{
    public class Sample : IEquatable<Sample>, ICloneable
    {
        public byte[] Values { get; set; }

        public int LastDistance { get; set; }

        public byte ModValue { get; set; }

        public byte TargetModValue { get; set; }

        public Sample(params byte[] _values)
        {
            Values = _values;

        }

        public void ForceChanges(byte _modFactor)
        {
            Values[Values.Length - 1] ^= 0x1;
        }

        public void UpdateModValue(byte _modFactor)
        {
            ModValue = (byte) (Values.Sum(val => val) & _modFactor);
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
            return new Sample((byte[]) Values.Clone()) {ModValue = ModValue};
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia.
        /// </summary>
        public static List<Sample> GetSampleListFrom(ICarrierMedia _carrierMedia, byte _modFactor)
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

                Sample sample = new Sample(sampleValues);
                sample.UpdateModValue(_modFactor);
                sampleList.Add(sample);
            }

            return sampleList;
        }
    }
}
