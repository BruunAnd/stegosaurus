using System;
using System.Collections.Generic;
using System.Linq;
using Stegosaurus.Carrier;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm
{
    public class Sample
    {
        public byte[] Values;

        public byte ModValue;
        public byte TargetModValue;

        public short LastDistance;

        public Sample(byte[] _values)
        {
            Values = _values;
        }

        public void UpdateModValue(byte _bitwiseModFactor)
        {
            ModValue = (byte) (Values.Sum(val => val) & _bitwiseModFactor);
        }

        public short DistanceTo(Sample _otherSample)
        {
            long distance = 0;

            for (int i = 0; i < _otherSample.Values.Length; i++)
            {
                distance += (int) Math.Pow(Values[i] - _otherSample.Values[i], 2);
            }

            LastDistance = distance > short.MaxValue ? short.MaxValue : (short) distance;

            return LastDistance;
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
            return new Sample((byte[]) Values.Clone()) { ModValue = ModValue, TargetModValue = TargetModValue };
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia.
        /// </summary>
        public static List<Sample> GetSampleListFrom(ICarrierMedia _carrierMedia, byte _bitwiseModFactor)
        {
            int bytesPerSample = _carrierMedia.BytesPerSample;
            List<Sample> sampleList = new List<Sample>(_carrierMedia.ByteArray.Length / _carrierMedia.BytesPerSample);

            int currentSample = 0;
            while (currentSample < _carrierMedia.ByteArray.Length)
            {
                byte[] sampleValues = new byte[bytesPerSample];

                for (int i = 0; i < bytesPerSample; i++)
                {
                    sampleValues[i] = _carrierMedia.ByteArray[currentSample++];
                }

                // Add new sample to list.
                Sample sample = new Sample(sampleValues);
                sample.UpdateModValue(_bitwiseModFactor);
                sampleList.Add(sample);
            }

            return sampleList;
        }

        /// <summary>
        /// Swap the values of this sample with another sample.
        /// </summary>
        public void Swap(Sample _otherSample)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                byte temp = Values[i];
                Values[i] = _otherSample.Values[i];
                _otherSample.Values[i] = temp;
            }
        }
    }
}
